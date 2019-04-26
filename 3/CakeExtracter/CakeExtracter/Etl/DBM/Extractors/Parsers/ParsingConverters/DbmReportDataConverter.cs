using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Models;
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
        public IEnumerable<DbmLineItemSummary> ConvertLineItemReportDataToSummaries(DbmAccountLineItemReportData reportData)
        {
            var account = reportData.Account;
            var lineItemSummaries = reportData.LineItemReportRows.Select(row => new DbmLineItemSummary
            {
                Date = row.Date,
                Cost = row.Revenue,
                Impressions = row.Impressions,
                Clicks = row.Clicks,
                PostClickConv = row.PostClickConv,
                PostViewConv = row.PostViewConv,
                CMPostClickRevenue = row.CMPostClickRevenue,
                CMPostViewRevenue = row.CMPostViewRevenue,
                LineItem = new DbmLineItem
                {
                    Account = account,
                    ExternalId = row.LineItemId,
                    Name = row.LineItemName,
                    Status = row.LineItemStatus,
                    Type = row.LineItemType,
                    InsertionOrder = new DbmInsertionOrder
                    {
                        Account = account,
                        ExternalId = row.InsertionOrderId,
                        Name = row.InsertionOrderName,
                        Campaign = new DbmCampaign
                        {
                            Account = account,
                            ExternalId = row.CampaignId,
                            Name = row.CampaignName,
                            Advertiser = new DbmAdvertiser
                            {
                                Account = account,
                                ExternalId = row.AdvertiserId,
                                Name = row.AdvertiserName,
                                Currency = row.AdvertiserCurrency
                            }
                        }
                    }
                }
            });
            return lineItemSummaries;
        }

        /// <summary>
        /// The method converts report rows of creatives to the list of creative summaries
        /// </summary>
        /// <param name="reportData">Part of the report data - creative rows and related account</param>
        /// <returns>List of creative summaries</returns>
        public IEnumerable<DbmCreativeSummary> ConvertCreativeReportDataToSummaries(
            DbmAccountCreativeReportData reportData)
        {
            var account = reportData.Account;
            var creativeSummaries = reportData.CreativeReportRows.Select(row => new DbmCreativeSummary
            {
                Date = row.Date,
                Cost = row.Revenue,
                Impressions = row.Impressions,
                Clicks = row.Clicks,
                PostClickConv = row.PostClickConv,
                PostViewConv = row.PostViewConv,
                CMPostClickRevenue = row.CMPostClickRevenue,
                CMPostViewRevenue = row.CMPostViewRevenue,
                Creative = new DbmCreative
                {
                    Account = account,
                    ExternalId = row.CreativeId,
                    Name = row.CreativeName,
                    Height = row.CreativeHeight,
                    Width = row.CreativeWidth,
                    Size = row.CreativeSize,
                    Type = row.CreativeType,
                    Advertiser = new DbmAdvertiser
                    {
                        Account = account,
                        ExternalId = row.AdvertiserId,
                        Name = row.AdvertiserName,
                        Currency = row.AdvertiserCurrency
                    }
                }
            });
            return creativeSummaries;
        }
    }
}
