using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apple;
using CakeExtracter.Common;
using CakeExtracter.Etl.Apple.Exceptions;

namespace CakeExtracter.Etl.Apple.Extractors
{
    /// <summary>
    /// Apple daily Ads stats extractor.
    /// </summary>
    public class AppleApiExtracter : Extracter<AppleStatGroup>
    {
        private const int MaxDaysNumberForSingleReport = 90;
        private readonly AppleAdsUtility appleAdsUtility;
        private readonly DateRange dateRange;
        private readonly string orgId;
        private readonly string certificateCode;

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<AppleFailedEtlException> ProcessFailedExtraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleApiExtracter"/> class.
        /// </summary>
        /// <param name="appleAdsUtility">Utility to extract items for a target account code <see cref="appleAdsUtility"/>.</param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="orgId">Database account code of the search account.</param>
        /// <param name="certificateCode">Database External Id of the search account.</param>
        public AppleApiExtracter(AppleAdsUtility appleAdsUtility, DateRange dateRange, string orgId, string certificateCode)
        {
            this.appleAdsUtility = appleAdsUtility;
            this.dateRange = dateRange;
            this.orgId = orgId;
            this.certificateCode = certificateCode;
        }

        //TODO: Handle gaps in the stats. Somehow pass deletion items... how to do this for all campaigns?

        /// <inheritdoc />
        /// <summary>
        /// Extracts the specified items.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info("Extracting stats from AppleAds API for ({0}) from {1:d} to {2:d}", orgId, dateRange.FromDate, dateRange.ToDate);
            try
            {
                var items = GetStatsDataForAllDates(dateRange.FromDate, dateRange.ToDate, GetStatsData);
                Add(items);
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, dateRange.FromDate, dateRange.ToDate);
            }
            finally
            {
                End();
            }
        }

        private IEnumerable<AppleStatGroup> GetStatsDataForAllDates(DateTime startDate, DateTime endDate, Func<DateTime, DateTime, IEnumerable<AppleStatGroup>> getItemsFunc)
        {
            const int daysNumberForNextStartDates = 1;
            var allItems = new List<AppleStatGroup>();
            var tasks = new List<Task>();

            while (startDate < endDate)
            {
                var nextDateRangeStartTime = startDate.AddDays(MaxDaysNumberForSingleReport);
                var endTime = endDate < nextDateRangeStartTime ? endDate : nextDateRangeStartTime;
                var startTime = startDate;
                var task = Task.Factory
                    .StartNew(() => getItemsFunc(startTime, endTime))
                    .ContinueWith(x => allItems.AddRange(x.Result));
                tasks.Add(task);
                startDate = endTime.AddDays(daysNumberForNextStartDates);
            }

            Task.WaitAll(tasks.ToArray());
            return allItems;
        }

        private IEnumerable<AppleStatGroup> GetStatsData(DateTime startDate, DateTime endDate)
        {
            Logger.Info("Extracting daily stats for ({0}) from {1:d} to {2:d}", orgId, startDate, endDate);
            var items = new List<AppleStatGroup>();
            try
            {
                items = appleAdsUtility.GetCampaignDailyStats(startDate, endDate, orgId, certificateCode).ToList();
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, startDate, endDate);
            }

            return items;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(e);
            var exception = new AppleFailedEtlException(fromDate, toDate, orgId, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
