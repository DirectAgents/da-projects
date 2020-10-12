using System;
using System.Linq;

using Amazon;
using Amazon.Entities.Summaries;
using AutoMapper;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonAttribution.Exceptions;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.AmazonAttribution;

namespace CakeExtracter.Etl.AmazonAttribution.Extractors
{
    /// <summary>
    /// Extractor to retrieve Amazon Attribution stats.
    /// </summary>
    public class AmazonAttributionExtractor : Extracter<AttributionSummary>
    {
        private readonly AmazonUtility amazonUtility;
        private readonly DateRange dateRange;
        private readonly ExtAccount account;

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<AttributionFailedEtlException> ProcessFailedExtraction;

        public AmazonAttributionExtractor(AmazonUtility amazonUtility, DateRange dateRange, ExtAccount account)
        {
            this.amazonUtility = amazonUtility;
            this.dateRange = dateRange;
            this.account = account;
        }

        protected override void Extract()
        {
            try
            {
                var advertisers = amazonUtility.GetAdvertisers(account.ExternalId);
                var data = amazonUtility.GetAmazonAttributionSummaries(
                    advertisers,
                    dateRange.FromDate,
                    dateRange.ToDate,
                    account.ExternalId);
                var summary = data.Select(TransformSummary);
                Add(summary);
            }
            catch (Exception e)
            {
                Logger.Error(account.Id, e);
                ProcessFailedStatsExtraction(e, dateRange.FromDate, dateRange.ToDate, account.Id);
            }
            finally
            {
                End();
            }
        }

        private AttributionSummary TransformSummary(AmazonAttributionSummary summary)
        {
            var item = Mapper.Map<AmazonAttributionSummary, AttributionSummary>(summary);
            item.AccountId = account.Id;
            return item;
        }

        private void ProcessFailedStatsExtraction(Exception e, DateTime? startDate, DateTime? endDate, int accountId)
        {
            Logger.Error(e);
            var exception = new AttributionFailedEtlException(startDate, endDate, accountId, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}