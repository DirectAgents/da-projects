using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models;
using SeleniumDataBrowser.PDA.Models.ConsoleManagerUtilityModels;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.PDA;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Extractors
{
    internal abstract class AmazonPdaExtractor<T> : BaseAmazonExtractor<T>
        where T : DatedStatsSummary
    {
        private readonly PdaDataProvider pdaDataProvider;

        protected AmazonPdaExtractor(ExtAccount account, DateRange dateRange, PdaDataProvider pdaDataProvider)
        : base(null, dateRange, account)
        {
            this.pdaDataProvider = pdaDataProvider;
        }

        public IEnumerable<AmazonPdaCampaignSummary> ExtractCampaignApiFullSummaries()
        {
            var apiCampaignSummaries = pdaDataProvider.GetCampaignApiFullSummaries(dateRange.Dates);
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