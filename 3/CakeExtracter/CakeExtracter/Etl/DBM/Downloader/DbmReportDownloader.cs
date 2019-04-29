using System.IO;
using System.Net;

namespace CakeExtracter.Etl.DBM.Downloader
{
    /// <summary>
    /// Downloader of DBM report contents
    /// </summary>
    public static class DbmReportDownloader
    {
        /// <summary>
        /// The method downloads a report content from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static StreamReader GetStreamReaderFromUrl(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var response = (HttpWebResponse) request.GetResponse();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);
            return streamReader;
        }
    }
}