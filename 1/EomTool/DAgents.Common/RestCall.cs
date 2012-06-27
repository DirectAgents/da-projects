using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DAgents.Common
{
    public class RestCall : IRestCall
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Response { get; set; }
        public bool NoCache { get; set; }

        public RestCall(string url)
        {
            Url = url;
            Headers = new Dictionary<string, string>();
            Method = "GET";
            NoCache = true;
        }

        public IRestCall Invoke()
        {
            var request = (HttpWebRequest)WebRequest.Create(this.Url);
            foreach (KeyValuePair<string, string> header in this.Headers)
            {
                string theHeader = header.Key + ":" + header.Value;
                request.Headers.Add(theHeader);
            }
            if (NoCache)
                request.Headers.Add("Cache-Control: no-cache");
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                this.Response = stream.ReadToEnd();
            }
            return this;
        }
    }
}
