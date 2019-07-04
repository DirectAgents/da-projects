using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.PDA;
using SeleniumDataBrowser.PDA.Models;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    /// <inheritdoc cref="BaseAmazonExtractor{T}"/>
    /// <summary>
    /// Campaign stats extractor for Amazon Product Display Ads.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class AmazonPdaExtractor<T> : BaseAmazonExtractor<T>
        where T : DatedStatsSummary
    {
        private readonly PdaDataProvider pdaDataProvider;

        /// <summary>
        /// Gets the display name for showing job execution state on history.
        /// </summary>
        public virtual string SummariesDisplayName { get; } = "Pda Summaries";

        /// <inheritdoc cref="BaseAmazonExtractor{T}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPdaExtractor{T}"/> class.
        /// </summary>
        /// <param name="account">Account for which stats will be extracted.</param>
        /// <param name="dateRange">Range of dates for which stats will be extracted.</param>
        /// <param name="pdaDataProvider">Amazon PDA data provider.</param>
        protected AmazonPdaExtractor(
            ExtAccount account,
            DateRange dateRange,
            PdaDataProvider pdaDataProvider)
            : base(null, dateRange, account)
        {
            this.pdaDataProvider = pdaDataProvider;
        }

        /// <summary>
        /// Returns summaries of Product Display Ads campaigns.
        /// </summary>
        /// <returns>Summaries of PDA campaigns.</returns>
        public IEnumerable<AmazonPdaCampaignSummary> ExtractPdaCampaignSummaries()
        {
            var apiCampaignSummaries = pdaDataProvider.GetPdaCampaignsSummaries(dateRange.Dates).ToList();
            AssignCampaignType(apiCampaignSummaries);
            var items = TransformSummaries(apiCampaignSummaries);
            return items;
        }

        private void AssignCampaignType(IEnumerable<AmazonCmApiCampaignSummary> apiCampaignSummaries)
        {
            var campaignType = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
            apiCampaignSummaries.ForEach(x => x.Type = campaignType);
        }

        private IEnumerable<AmazonPdaCampaignSummary> TransformSummaries(IEnumerable<AmazonCmApiCampaignSummary> apiSummaries)
        {
            return apiSummaries.Select(CreateSummary);
        }

        private AmazonPdaCampaignSummary CreateSummary(AmazonCmApiCampaignSummary apiSummary)
        {
            var summary = new AmazonPdaCampaignSummary
            {
                Date = apiSummary.Date,
                Id = apiSummary.Id,
                Name = apiSummary.Name,
                Type = apiSummary.Type,
                TargetingType = apiSummary.TargetingType,
                Status = apiSummary.Status,
                StartDate = apiSummary.StartDate,
                EndDate = apiSummary.EndDate,
                CreationDate = apiSummary.CreationDate,
                Orders = apiSummary.Orders,
                DetailPageViews = apiSummary.DetailPageViews,
                UnitsSold = apiSummary.UnitsSold,
                Impressions = apiSummary.Impressions,
                Clicks = apiSummary.Clicks,
                Cost = apiSummary.Cost,
                AttributedSales14D = apiSummary.AttributedSales14D,
            };
            return summary;
        }
    }
}