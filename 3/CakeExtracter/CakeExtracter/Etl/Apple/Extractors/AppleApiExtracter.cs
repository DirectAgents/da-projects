using System;
using System.Collections.Generic;
using System.Linq;
using Apple;
using CakeExtracter.Common;
using CakeExtracter.Etl.Apple.Exceptions;

namespace CakeExtracter.Etl.Apple.Extractors
{
    public class AppleApiExtracter : Extracter<AppleStatGroup>
    {
        protected readonly AppleAdsUtility appleAdsUtility;
        protected readonly DateRange dateRange;
        protected readonly string orgId;
        protected readonly string certificateCode;

        public event Action<AppleFailedEtlException> ProcessFailedExtraction;

        public AppleApiExtracter(AppleAdsUtility appleAdsUtility, DateRange dateRange, string orgId, string certificateCode)
        {
            this.appleAdsUtility = appleAdsUtility;
            this.dateRange = dateRange;
            this.orgId = orgId;
            this.certificateCode = certificateCode;
        }

        //TODO: Handle gaps in the stats. Somehow pass deletion items... how to do this for all campaigns?

        protected override void Extract()
        {
            Logger.Info("Extracting daily stats from AppleAds API for ({0}) from {1:d} to {2:d}", orgId, dateRange.FromDate, dateRange.ToDate);
            try
            {
                var start = dateRange.FromDate;
                var end = dateRange.ToDate;
                var date90daysAfterStart = start.AddDays(90);
                if (end > date90daysAfterStart)
                {
                    end = date90daysAfterStart;
                }

                while (start <= dateRange.ToDate)
                {
                    var appleStatGroups = GetDailyStats(start, end);

                    if (appleStatGroups != null)
                    {
                        Add(appleStatGroups);
                    }

                    start = start.AddDays(91);
                    end = end.AddDays(91);
                    if (end > dateRange.ToDate)
                    {
                        end = dateRange.ToDate;
                    }
                }
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

        private IEnumerable<AppleStatGroup> GetDailyStats(DateTime startDate, DateTime endDate)
        {
            var appleStatGroups = new List<AppleStatGroup>();
            try
            {
                //throw new Exception("Failed extract!");
                appleStatGroups = appleAdsUtility.GetCampaignDailyStats(startDate, endDate, orgId, certificateCode).ToList();
            }
            catch (Exception e)
            {
                ProcessFailedStatsExtraction(e, startDate, endDate);
            }

            return appleStatGroups;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate)
        {
            Logger.Error(e);
            var exception = new AppleFailedEtlException(fromDate, toDate, orgId, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
