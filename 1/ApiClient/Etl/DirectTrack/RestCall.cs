using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Spring.Http.Client.Interceptor;
using Spring.Rest.Client;
using Spring.Util;
using System.Linq;

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

                if (!url.EndsWith("/"))
                {
                    restTemplate.RequestInterceptors.Add(new NoCacheInterceptor());
                }

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
}
