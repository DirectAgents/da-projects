using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Models;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors.Parsers.ParsingConverters
{
    /// <summary>DBM report data composer.</summary>
    //internal class DbmReportDataComposer<T> where T: DbmBaseReportRow
    internal class DbmReportDataConverter
    {
        public IEnumerable<DbmLineItemSummary> ConvertLineItemReportDataToSummaries(DbmAccountLineItemReportData reportData)
        {
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
                    InsertionOrder = new DbmInsertionOrder
                    {
                        Campaign = new DbmCampaign
                        {
                            Advertiser = new DbmAdvertiser
                            {
                                Account = reportData.Account,
                                ExternalId = row.AdvertiserId,
                                Name = row.AdvertiserName,
                                Currency = row.AdvertiserCurrency
                            },
                            ExternalId = row.CampaignId,
                            Name = row.CampaignName
                        },
                        ExternalId = row.InsertionOrderId,
                        Name = row.InsertionOrderName
                    },
                    ExternalId = row.LineItemId,
                    Name = row.LineItemName
                }
            });
            return lineItemSummaries;
        }

        //public IEnumerable<DbmCreativeSummary> ConvertCreativeReportDataToSummaries(DbmAccountCreativeReportData reportData)
        //{
        //    return reportData.CreativeReportRows.Select(row => new DbmCreativeSummary
        //    {
        //        Creative = new DbmCreative
        //        {
        //            LineItem = new DbmLineItem
        //            {
        //                InsertionOrder = new DbmInsertionOrder
        //                {
        //                    Campaign = new DbmCampaign
        //                    {
        //                        Advertiser = new DbmAdvertiser
        //                        {
        //                            Account = reportData.Account,
        //                            ExternalId = row.AdvertiserId,
        //                            Name = row.AdvertiserName,
        //                            Currency = row.AdvertiserCurrency
        //                        },
        //                        ExternalId = row.CampaignId,
        //                        Name = row.CampaignName
        //                    },
        //                    ExternalId = row.InsertionOrderId,
        //                    Name = row.InsertionOrderName
        //                },
        //                ExternalId = row.LineItemId,
        //                Name = row.LineItemName
        //            },
        //            ExternalId = row.CreativeId,
        //            Name = row.CreativeName,
        //            Height = row.CreativeHeight,
        //            Width = row.CreativeWidth,
        //            Size = row.CreativeSize,
        //            Type = row.CreativeType
        //        },
        //        Date = row.Date,
        //        Cost = row.Revenue,
        //        Impressions = row.Impressions,
        //        Clicks = row.Clicks,
        //        PostClickConv = row.PostClickConv,
        //        PostViewConv = row.PostViewConv,
        //        CMPostClickRevenue = row.CMPostClickRevenue,
        //        CMPostViewRevenue = row.CMPostViewRevenue
        //    });
        //}
    }
}
