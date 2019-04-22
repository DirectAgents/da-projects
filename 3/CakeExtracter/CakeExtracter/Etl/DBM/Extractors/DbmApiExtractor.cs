using System.IO;
using System.Net;
using DBM;

namespace CakeExtracter.Etl.DBM.Extractors
{
    public abstract class DbmApiExtractor<T> : Extracter<T>
    {
        protected readonly DBMUtility DbmUtility;

        public DbmApiExtractor(DBMUtility dbmUtility)
        {
            DbmUtility = dbmUtility;
        }

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