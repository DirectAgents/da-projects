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
        private readonly int? insertionOrderId; // null for all
        private readonly bool includeViewThrus; // (false = only include click-thrus)

        public DbmConversionExtracter(DateRange dateRange, string bucketName, int? insertionOrderId, bool includeViewThrus)
        {
            this.dateRange = dateRange;
            this.bucketName = bucketName;
            this.insertionOrderId = insertionOrderId;
            this.includeViewThrus = includeViewThrus;
        }

        protected override void Extract()
        {
            string includeWhat = includeViewThrus ? "all" : "click-thru";
            Logger.Info("Extracting {0} Conversions from Data Transfer Files for {1} from {2} to {3}", includeWhat, bucketName, dateRange.FromDate, dateRange.ToDate);
            if (insertionOrderId.HasValue)
                Logger.Info("InsertionOrder {0}", insertionOrderId);
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
                Logger.Info("Date: {0}", date);

                string filename = String.Format("log/{0}.0.conversion.0.csv", date.ToString("yyyyMMdd"));
                //listRequest.Prefix = filename;
                //var matchingObjects = listRequest.Execute();
                //if (matchingObjects.Items == null)
                //    continue;

                //var reportObject = bucketObjects.Items.Where(i => i.Name == filename).FirstOrDefault();

                var request = service.Objects.Get(bucketName, filename);
                var obj = request.Execute();
                if (obj != null)
                {
                    var stream = DbmCloudStorageExtracter.GetStreamForCloudStorageObject(obj, credential);
                    using (var reader = new StreamReader(stream))
                    {
                        foreach (var row in EnumerateRowsStatic(reader))
                            yield return row;
                    }
                }
            }
        }

        public IEnumerable<DataTransferRow> EnumerateRowsStatic(StreamReader reader)
        {
            using (CsvReader csv = new CsvReader(reader))
            {
                var csvRows = csv.GetRecords<DataTransferRow>().ToList();
                for (int i = 0; i < csvRows.Count; i++)
                {
                    if (insertionOrderId.HasValue && insertionOrderId.Value != csvRows[i].insertion_order_id)
                        continue;
                    if (!includeViewThrus && csvRows[i].event_sub_type == "postview")
                        continue;
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

        public string event_type { get; set; }
        public string event_sub_type { get; set; }
        public string user_id { get; set; }
        public int? ad_position { get; set; }
        public string ip { get; set; }
        public string country { get; set; }
        public int? dma_code { get; set; }
        public string postal_code { get; set; }
        public int? geo_region_id { get; set; }
        public int? city_id { get; set; }
        public int? os_id { get; set; }
        public int? browser_id { get; set; }
        public int? browser_timezone_offset_minutes { get; set; }
        public int? net_speed { get; set; }
    }
}
