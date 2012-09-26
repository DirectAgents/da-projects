using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Spring.Http.Client.Interceptor;
using Spring.Rest.Client;
using Spring.Util;

namespace DirectTrack
{
    public class RestCall : IRestCall
    {
        public ApiInfo DirectTrackRestApiInfo { get; set; }
        public Dictionary<string, object> UriVariables { get; set; }

        public RestCall(ApiInfo apiInfo)
        {
            this.DirectTrackRestApiInfo = apiInfo;
            this.UriVariables = new Dictionary<string, object>();
        }

        public string GetXml(string url, out Uri expandedUri)
        {
            UriTemplate uriTemplate = new UriTemplate(url);
            expandedUri = uriTemplate.Expand(this.UriVariables);

            RestTemplate restTemplate = new RestTemplate(this.DirectTrackRestApiInfo.BaseUrl);
            restTemplate.RequestInterceptors.Add(new BasicSigningRequestInterceptor(this.DirectTrackRestApiInfo.Login, this.DirectTrackRestApiInfo.Password));

            if (!url.EndsWith("/"))
            {
                restTemplate.RequestInterceptors.Add(new NoCacheInterceptor());
            }

            string xml = restTemplate.GetForObject<string>(url, this.UriVariables);

            using (var memoryStream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(xml)))
            using (var sanitizingStream = new XmlSanitizingStream(memoryStream))
            {
                xml = sanitizingStream.ReadToEnd();
            }

            return xml;
        }

        public T GetXml<T>(string url, out Uri expandedUri)
        {
            string xml = this.GetXml(url, out expandedUri);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new System.IO.StringReader(xml));
        }
    }
}
