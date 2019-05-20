using System.IO;
using System.Net;

namespace CakeExtracter.Helpers
{
    /// <summary>
    /// The utility contains methods to help work with web requests.
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// Create stream reader for content by url.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Specified stream</returns>
        public static StreamReader CreateStreamReaderFromUrl(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);
            return streamReader;
        }
    }
}
