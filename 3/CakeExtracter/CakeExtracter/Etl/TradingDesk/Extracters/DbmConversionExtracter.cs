using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CakeExtracter.Common;
using CsvHelper;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class DbmConversionExtracter : Extracter<DataTransferRow>
    {
        private readonly DateRange dateRange;
        private readonly string bucketName;

        public DbmConversionExtracter(DateRange dateRange, string bucketName)
        {
            this.dateRange = dateRange;
            this.bucketName = bucketName;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting Conversions from Data Transfer Files for {0} from {1} to {2}", bucketName, dateRange.FromDate, dateRange.ToDate);
            var items = EnumerateRows();
            Add(items);
            End();
        }

        private IEnumerable<DataTransferRow> EnumerateRows()
        {
            var credential = DbmCloudStorageExtracter.CreateCredential();
            var service = DbmCloudStorageExtracter.CreateStorageService(credential);

            //var listRequest = service.Objects.List(bucketName);
            //var bucketObjects = listRequest.Execute();

            foreach (var date in dateRange.Dates)
            {
                string filename = String.Format("log/{0}.0.conversion.0.csv", date.ToString("yyyyMMdd"));
                //listRequest.Prefix = filename;
                //var matchingObjects = listRequest.Execute();
                //if (matchingObjects.Items == null)
                //    continue;

                //var reportObject = bucketObjects.Items.Where(i => i.Name == filename).FirstOrDefault();

                var request = service.Objects.Get(bucketName, filename);
                var obj = request.Execute();

                HttpWebRequest req = DbmCloudStorageExtracter.CreateRequest(obj.MediaLink, credential);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                // Handle redirects manully to ensure that the Authorization header is present if
                // our request is redirected.
                if (resp.StatusCode == HttpStatusCode.TemporaryRedirect)
                {
                    req = DbmCloudStorageExtracter.CreateRequest(resp.Headers["Location"], credential);
                    resp = (HttpWebResponse)req.GetResponse();
                }

                Stream stream = resp.GetResponseStream();
                using (var reader = new StreamReader(stream))
                {
                    foreach (var row in EnumerateRowsStatic(reader))
                        yield return row;
                }
            }
        }

        public static IEnumerable<DataTransferRow> EnumerateRowsStatic(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                var csvRows = csv.GetRecords<DataTransferRow>().ToList();
                for (int i = 0; i < csvRows.Count; i++)
                {
                    yield return csvRows[i];
                }
            }
        }
    }

    // TODO: handle blank event_time / auction_id... skip that row?

    public class DataTransferRow
    {
        public string auction_id { get; set; }
        public long event_time { get; set; }
        public long? view_time { get; set; }
        public long? request_time { get; set; }
        public int? insertion_order_id { get; set; }
        public int? line_item_id { get; set; }
        public int? creative_id { get; set; }
    }
}
