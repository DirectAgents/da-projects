using System;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.Criteo.Exceptions;

namespace CakeExtracter.Etl.TradingDesk.Extracters.CriteoExtractors
{
    public class CriteoDatabaseStrategyToDailySummaryExtractor : DatabaseStrategyToDailySummaryExtractor
    {
        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<CriteoFailedEtlException> ProcessFailedExtraction;

        public CriteoDatabaseStrategyToDailySummaryExtractor (DateRange dateRange, int accountId)
            : base(dateRange, accountId)
        {
        }

        protected override void Extract()
        {
            try
            {
                base.Extract();
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, dateRange.FromDate, dateRange.ToDate, accountId, StatsTypeAgg.DailyArg);
            }
        }

        protected void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate, int accountId, string statsType)
        {
            Logger.Error(e);
            var exception = new CriteoFailedEtlException(fromDate, toDate, accountId, e, statsType);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
