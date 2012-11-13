using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using ApiClient.Models.DirectTrack;
using Spring.Http.Client.Interceptor;
using Spring.Rest.Client;
using Spring.Util;

namespace DirectTrack
{
    public class RestCall : IRestCall
    {
        private ILogger logger;
        public ApiInfo DirectTrackRestApiInfo { get; set; }
        public Dictionary<string, object> UriVariables { get; set; }

        public RestCall(ApiInfo apiInfo, ILogger logger)
        {
            this.DirectTrackRestApiInfo = apiInfo;
            this.UriVariables = new Dictionary<string, object>();
            this.logger = logger;
        }

        public string GetXml(string url, out Uri expandedUri, out bool cached)
        {
            UriTemplate uriTemplate = new UriTemplate(url);
            expandedUri = uriTemplate.Expand(this.UriVariables);

            string xml = GetCachedResult(url);

            if (xml == null)
            {
                cached = false;
                logger.Log("GET: " + url);

                RestTemplate restTemplate = new RestTemplate(this.DirectTrackRestApiInfo.BaseUrl);
                restTemplate.RequestInterceptors.Add(new BasicSigningRequestInterceptor(this.DirectTrackRestApiInfo.Login, this.DirectTrackRestApiInfo.Password));
                //restTemplate.RequestInterceptors.Add(new LogDirectTrackApiCallInterceptor());
                //if (!url.EndsWith("/"))
                //{
                //    restTemplate.RequestInterceptors.Add(new NoCacheInterceptor());
                //}

                xml = restTemplate.GetForObject<string>(url, this.UriVariables);

                using (var memoryStream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(xml)))
                using (var sanitizingStream = new XmlSanitizingStream(memoryStream))
                {
                    xml = sanitizingStream.ReadToEnd();
                }
            }
            else
            {
                cached = true;
                logger.Log("USING CACHED: " + url);
            }

            return xml;
        }

        private string GetCachedResult(string url)
        {
            using (var db = new ApiClient.Models.DirectTrack.DirectTrackDbContext())
            {
                return db.DirectTrackResources.Where(c => c.Name == url).Select(c => c.Content).FirstOrDefault();
            }
        }

        public T GetXml<T>(string url, out Uri expandedUri, out bool cached)
        {
            string xml = this.GetXml(url, out expandedUri, out cached);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new System.IO.StringReader(xml));
        }
    }

    public class LogDirectTrackApiCallInterceptor : IClientHttpRequestSyncInterceptor
    {
        public Spring.Http.Client.IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
        {
            using (var db = new DirectTrackDbContext())
            {
                var existing = db.DirectTrackApiCalls.FirstOrDefault(c => c.Url == execution.Uri.AbsoluteUri);
                if (existing != null)
                {
                    Console.WriteLine("Already made call to " + execution.Uri.AbsoluteUri + " which resulted in " + existing.Status);
                    return null;
                }
            }

            int points = GetPointsUsed(1);
            while (points > 3000)
            {
                Console.WriteLine("Points at " + points + ", Sleeping for a minute...");
                Thread.Sleep(60 * 1000);
                points = GetPointsUsed(1);
            }

            var result = execution.Execute();

            using (var db = new DirectTrackDbContext())
            {
                int pointsUsed = 10;

                if (execution.Uri.AbsoluteUri.EndsWith("/")) // Resource lists end with '/' and cost 1 point per resourceURL
                {
                    XNamespace dt = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0";

                    var reader = new StreamReader(result.Body);
                    string xmlString = reader.ReadToEnd();
                    //reader.Close();

                    XDocument doc = XDocument.Parse(xmlString);

                    var resourceUrlElements =
                        from c in doc.Root.Elements(dt + "resourceURL")
                        select c;

                    pointsUsed = resourceUrlElements.Count();
                }

                var apiCall = new DirectTrackApiCall
                {
                    Url = execution.Uri.AbsoluteUri,
                    Status = ((int)result.StatusCode).ToString() + " " + result.StatusDescription,
                    Timestamp = DateTime.Now,
                    PointsUsed = pointsUsed
                };

                db.DirectTrackApiCalls.Add(apiCall);
                db.SaveChanges();

                Console.WriteLine("Logged " + pointsUsed + " points..");
            }

            return result;
        }

        private static int GetPointsUsed(int minutes)
        {
            using (var db = new DirectTrackDbContext())
            {
                int result = 0;
                var ago = DateTime.Now.AddMinutes(-1 * minutes);
                var points = (from c in db.DirectTrackApiCalls
                              where c.Timestamp >= ago
                              select c.PointsUsed)
                            .ToList();
                if (points.Count > 0)
                    result = points.Sum();
                return result;
            }
        }
    }
}
