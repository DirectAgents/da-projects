using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using ApiClient.Models.DirectTrack;
using Common;
using DirectTrack;

namespace ApiClient.Etl.DirectTrack
{
    public class ResourcesFromDirectTrack : Source<DirectTrackResource>
    {
        private string url;
        private ResourceGetterMode mode;
        private int limit;
        private DateRange dateRange;
        private ThreadMode threadMode;
        private int batchSize;

        public ResourcesFromDirectTrack(string url,
                                        ResourceGetterMode mode,
                                        int limit = int.MaxValue,
                                        DateRange dateRange = null,
                                        ThreadMode threadMode = ThreadMode.Single,
                                        int batchSize = 10)
        {
            this.url = url;
            this.mode = mode;
            this.limit = limit;
            this.dateRange = dateRange;
            this.threadMode = threadMode;
            this.batchSize = batchSize;
        }

        public override void DoExtract()
        {
            bool containsCampaignID = url.Contains("[campaign_id]");
            bool containsDate = url.Contains("[yyyy]-[mm]-[dd]") || url.Contains("[yyyy]-[mm]");

            if (containsDate && this.dateRange == null)
                throw new Exception("DateRange required when url contains '[yyyy]-[mm]-[dd]'");

            List<string> urlsWithPID = null;

            if (containsCampaignID)
            {
                urlsWithPID = Pids.Select(pid => url.Replace("[campaign_id]", pid.ToString())).ToList();
            }

            Func<string, DateTime, string> substituteDate = (c, date) =>
            {
                return c.Replace("[yyyy]", date.ToString("yyyy"))
                        .Replace("[mm]", date.ToString("MM"))
                        .Replace("[dd]", date.ToString("dd"));
            };

            if (containsCampaignID && containsDate)
            {
                var count = urlsWithPID.Count();
                for (int i = 0; i < (urlsWithPID.Count() / batchSize) + 1; i++)
                {
                    var batch = urlsWithPID.Skip(i * batchSize).Take(batchSize);
                    ForEach(batch, urlWithPID =>
                    {
                        Logger.Log("Working on {0}", urlWithPID);
                        var urls = dateRange.Dates.Select(date => substituteDate(urlWithPID, date)).ToList();
                        Logger.Log("Fetching {0} urls...", urls.Count);
                        DoGetResources(urls);
                    });
                }
            }
            else if (containsCampaignID)
            {
                DoGetResources(urlsWithPID);
            }
            else if (containsDate)
            {
                var urls = dateRange.Dates.Select(date => substituteDate(url, date)).ToList();
                DoGetResources(urls);
            }
            else
            {
                GetResources(url);
            }

            Done = true;
        }

        void ForEach<T>(IEnumerable<T> batch, Action<T> action)
        {
            if (this.threadMode == ThreadMode.Single)
                foreach (var item in batch)
                    action(item);
            else
                Parallel.ForEach(batch, item => action(item));
        }

        private void DoGetResources(List<string> urls)
        {
            urls.ForEach(c =>
            {
                if (Paused)
                    WaitUntilNotPaused();

                GetResources(c);
            });
        }

        private void GetResources(string url)
        {
            var logger = new ConsoleLogger();
            var restCall = new RestCall(new ApiInfo(), logger);
            var getter = new ResourceGetter(logger, url, restCall, limit, this.mode);
            var errors = new List<string>();
            getter.GotResource += (sender, uri, url2, doc, cached) =>
            {
                if (!cached)
                {
                    var newResource = new DirectTrackResource
                    {
                        Name = url2,
                        Content = doc.ToString(),
                        Timestamp = DateTime.UtcNow,
                        AccessId = ApiInfo.LoginAccessId,
                    };

                    newResource.PointsUsed = 10;

                    if (url2.EndsWith("/")) // Resource lists end with '/' and cost 1 point per resourceURL
                    {
                        XNamespace dt = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0";

                        var resourceUrlElements =
                            from c in doc.Root.Elements(dt + "resourceURL")
                            select c;

                        newResource.PointsUsed += resourceUrlElements.Count();
                    }

                    AddItems(new[] { newResource });
                }
            };
            getter.Error += (sender, uri, ex) =>
            {
                Logger.Log("Exception: {0}", ex.Message);
                errors.Add(url + " - " + ex.Message);
            };
            getter.Run();
        }

        int[] Pids
        {
            get
            {
                return new [] { 31 };
                //using (var db = new Models.DirectTrack.ModelFirst.DirectTrackEntities1())
                //{
                //    return db.campaign_stats
                //             .OrderByDescending(c => c.Leads)
                //             .Select(c => c.PID)
                //             .ToArray();
                //}
            }
        }
    }
}
