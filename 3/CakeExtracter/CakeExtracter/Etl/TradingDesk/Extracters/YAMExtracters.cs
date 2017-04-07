using System.IO;
using System.Net;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using Yahoo;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class YAMApiExtracter<T> : Extracter<T>
    {
        protected YAMUtility _yamUtility;
        protected DateRange dateRange;
        protected ColumnMapping columnMapping;
        protected int yamAdvertiserId;

        public YAMApiExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
        {
            this._yamUtility = yamUtility;
            this.dateRange = dateRange;
            this.columnMapping = account.Platform.PlatColMapping;
            this.yamAdvertiserId = int.Parse(account.ExternalId);
        }

        public static StreamReader CreateStreamReaderFromUrl(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream);
            return streamReader;
        }
    }

    public class YAMDailySummaryExtracter : YAMApiExtracter<DailySummary>
    {
        public YAMDailySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        {
        }

        protected override void Extract()
        {
            var response = _yamUtility.RequestReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId);
            string reportUrl = _yamUtility.ObtainReportUrl(response);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDDailySummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
                End();
            }
        }
    }

    public class YAMStrategySummaryExtracter : YAMApiExtracter<StrategySummary>
    {
        public YAMStrategySummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        {
        }

        protected override void Extract()
        {
            var response = _yamUtility.RequestReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byLine: true);
            string reportUrl = _yamUtility.ObtainReportUrl(response);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDStrategySummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
                End();
            }
        }
    }

    public class YAMTDadSummaryExtracter : YAMApiExtracter<TDadSummary>
    {
        public YAMTDadSummaryExtracter(YAMUtility yamUtility, DateRange dateRange, ExtAccount account)
            : base(yamUtility, dateRange, account)
        {
        }

        protected override void Extract()
        {
            var response = _yamUtility.RequestReport(dateRange.FromDate, dateRange.ToDate, this.yamAdvertiserId, byAd: true);
            string reportUrl = _yamUtility.ObtainReportUrl(response);

            if (!string.IsNullOrWhiteSpace(reportUrl))
            {
                var streamReader = CreateStreamReaderFromUrl(reportUrl);
                var tdExtracter = new TDadSummaryExtracter(this.columnMapping, streamReader: streamReader);
                var items = tdExtracter.EnumerateRows();
                Add(items);
                End();
            }
        }
    }
}
