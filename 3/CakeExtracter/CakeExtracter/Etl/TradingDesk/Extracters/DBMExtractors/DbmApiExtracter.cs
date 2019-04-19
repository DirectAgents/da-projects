using System.IO;
using System.Net;

namespace CakeExtracter.Etl.TradingDesk.Extracters.DbmExtractors
{
    public abstract class DbmApiExtracter<T> : Extracter<T>
    {
        public static StreamReader CreateStreamReaderFromUrl(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var response = (HttpWebResponse) request.GetResponse();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);
            return streamReader;
        }
    }
}