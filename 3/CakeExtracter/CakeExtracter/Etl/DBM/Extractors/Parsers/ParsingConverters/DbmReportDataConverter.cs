using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters
{
    /// <summary>DBM report data converter.</summary>
    internal class DbmReportDataConverter
    {
        /// <summary>
        /// The method converts report rows of line items to the list of line item summaries
        /// </summary>
        /// <param name="reportData">Part of the report data - line item rows and related account</param>
        /// <returns>List of line item summaries</returns>
        public List<DbmLineItemSummary> ConvertLineItemReportDataToSummaries(DbmAccountLineItemReportData reportData)
        {
            var lineItemSummaries = reportData.LineItemReportRows.Select(row => GetLineItemSummary(reportData.Account, row));
            return lineItemSummaries.ToList();
        }
        
        /// <summary>
        /// The method converts report rows of creatives to the list of creative summaries
        /// </summary>
        /// <param name="reportData">Part of the report data - creative rows and related account</param>
        /// <returns>List of creative summaries</returns>
        public List<DbmCreativeSummary> ConvertCreativeReportDataToSummaries(DbmAccountCreativeReportData reportData)
        {
            var creativeSummaries = reportData.CreativeReportRows.Select(row => GetCreativeSummary(reportData.Account, row));
            return creativeSummaries.ToList();
        }

        private static DbmCreativeSummary GetCreativeSummary(ExtAccount account, DbmCreativeReportRow row)
        {
            var summary = new DbmCreativeSummary
            {
                Creative = GetCreative(account, row)
            };
            SetMetricsForSummary(summary, row);
            return summary;
        }

        private static DbmLineItemSummary GetLineItemSummary(ExtAccount account, DbmLineItemReportRow row)
        {
            var summary = new DbmLineItemSummary
            {
                LineItem = GetLineItem(account, row)
            };
            SetMetricsForSummary(summary, row);
            return summary;
        }

        private static void SetMetricsForSummary(DbmBaseSummaryEntity summary, DbmBaseReportRow row)
        {
            summary.Date = row.Date;
            summary.Revenue = row.Revenue;
            summary.Impressions = row.Impressions;
            summary.Clicks = row.Clicks;
            summary.PostClickConversions = row.PostClickConversions;
            summary.PostViewConversions = row.PostViewConversions;
            summary.CMPostClickRevenue = row.CMPostClickRevenue;
            summary.CMPostViewRevenue = row.CMPostViewRevenue;
        }

        private static DbmCreative GetCreative(ExtAccount account, DbmCreativeReportRow row)
        {
            return new DbmCreative
            {
                Account = account,
                ExternalId = row.CreativeId,
                Name = row.CreativeName,
                Height = int.TryParse(row.CreativeHeight, out var creativeHeight) ? creativeHeight : (int?)null,
                Width = int.TryParse(row.CreativeWidth, out var creativeWidth) ? creativeWidth : (int?) null,
                Size = row.CreativeSize,
                Type = row.CreativeType,
                Advertiser = GetAdvertiser(account, row.AdvertiserId, row.AdvertiserName, row.AdvertiserCurrencyCode)
            };
        }
        
        private static DbmLineItem GetLineItem(ExtAccount account, DbmLineItemReportRow row)
        {
            return new DbmLineItem
            {
                Account = account,
                ExternalId = row.LineItemId,
                Name = row.LineItemName,
                Status = row.LineItemStatus,
                Type = row.LineItemType,
                InsertionOrder = GetInsertionOrder(account, row)
            };
        }

        private static DbmInsertionOrder GetInsertionOrder(ExtAccount account, DbmBaseReportRow row)
        {
            return new DbmInsertionOrder
            {
                Account = account,
                ExternalId = row.InsertionOrderId,
                Name = row.InsertionOrderName,
                Campaign = GetCampaign(account, row)
            };
        }

        private static DbmCampaign GetCampaign(ExtAccount account, DbmBaseReportRow row)
        {
            return new DbmCampaign
            {
                Account = account,
                ExternalId = row.CampaignId,
                Name = row.CampaignName,
                Advertiser = GetAdvertiser(account, row.AdvertiserId, row.AdvertiserName, row.AdvertiserCurrencyCode)
            };
        }

        private static DbmAdvertiser GetAdvertiser(ExtAccount account, string id, string name, string currencyCode)
        {
            return new DbmAdvertiser
            {
                Account = account,
                ExternalId = id,
                Name = name,
                CurrencyCode = currencyCode
            };
        }
    }
}
