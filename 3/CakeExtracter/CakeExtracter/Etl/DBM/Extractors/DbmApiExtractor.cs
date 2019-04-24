using System.IO;
using System.Net;
using CakeExtracter.Common;
using DBM;

namespace CakeExtracter.Etl.DBM.Extractors
{
    public abstract class DbmApiExtractor<T> : Extracter<T>
    {
        //protected readonly DBMUtility _dbmUtility;
        //protected readonly DateRange? _dateRange;

        //public DbmApiExtractor(DBMUtility dbmUtility, DateRange? dateRange)
        //{
        //    _dbmUtility = dbmUtility;
        //    _dateRange = dateRange;
        //}

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