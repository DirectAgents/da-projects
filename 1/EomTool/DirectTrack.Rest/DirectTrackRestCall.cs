using System;
using System.Text;
using System.Xml;
using DAgents.Common;

namespace DirectTrack.Rest
{
    public class DirectTrackRestCall : RestCall
    {
        private DirectTrackRestCall(string url)
            : base(url)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("directagents:Il1K3m0N3y");
            string auth = "Basic " + Convert.ToBase64String(bytes);
            this.Headers.Add("Authorization", auth);
        }

        // Takes a URL to a DirectTrack REST API endpoint (GET) and returns an instance
        // DirectTrackRestCall configured to retrieve the response.
        public static DirectTrackRestCall Create(string url)
        {
            return new DirectTrackRestCall(url);
        }

        // Takes a URL to a DirectTrack REST API endpoint (GET) and retrieves the response as
        // a string.
        public static string GetXml(string url)
        {
            return Create(url).Invoke().Response;
        }

        // Takes a URL to a DirectTrack REST API endpoint (GET) and retrieves the response 
        // as an XmlDocument.
        public static XmlDocument GetXmlDoc(string url)
        {
            string xml = GetXml(url);
            var xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            return xdoc;
        }
    }
}
