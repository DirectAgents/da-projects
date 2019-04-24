using System.IO;
using System.Net;

namespace CakeExtracter.Etl.DBM.Downloader
{
    public static class DbmReportDownloader
    {
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