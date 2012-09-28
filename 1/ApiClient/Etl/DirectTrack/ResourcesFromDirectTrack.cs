using System;
using System.Linq;
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

        public ResourcesFromDirectTrack(string url, ResourceGetterMode mode, int limit = int.MaxValue)
        {
            this.url = url;
            this.mode = mode;
            this.limit = limit;
        }

        public override void DoExtract()
        {
            if (url.Contains("[campaign_id]"))
                Array.ForEach(Pids, pid => GetResources(url.Replace("[campaign_id]", pid.ToString())));
            else
                GetResources(url);
            Done = true;
        }

        private void GetResources(string url)
        {
            var logger = new ConsoleLogger();
            var restCall = new RestCall(new ApiInfo(), logger);
            var getter = new ResourceGetter(logger, url, restCall, limit, this.mode);
            getter.GotResource += (sender, uri, url2, doc, cached) =>
            {
                if (!cached)
                    AddItems(new[] { new DirectTrackResource 
                    {
                        Name = url2, 
                        Content = doc.ToString(),
                        Timestamp = DateTime.UtcNow
                    }});
            };
            getter.Error += (sender, uri, ex) => Logger.Log("Exception: ", ex.Message);
            getter.Run();
        }

        int[] Pids
        {
            get
            {
                using (var db = new DirectTrackDbContext())
                {
                    var query = from c in db.DirectTrackResources
                                where c.Name.Contains("1/campaign/../../campaign")
                                select c.Name;
                    var pids = query.AsEnumerable().Select(c => c.Split('/').Last());
                    return pids.Select(c => int.Parse(c)).ToArray();
                }
            }
        }
    }
}
