using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.Utilities;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    internal abstract class AmazonPdaExtractor<T> : BaseAmazonExtractor<T>
        where T : DatedStatsSummary
    {
        private readonly PdaDataProvider pdaDataProvider;

        protected AmazonPdaExtractor(ExtAccount account, DateRange dateRange)
        : base(null, dateRange, account)
        {
            this.pdaDataProvider = new PdaDataProvider(account);
        }

        public IEnumerable<AmazonCmApiCampaignSummary> ExtractCampaignApiFullSummaries()
        {
            return pdaDataProvider.GetCampaignApiFullSummaries(dateRange);
        }
    }
}