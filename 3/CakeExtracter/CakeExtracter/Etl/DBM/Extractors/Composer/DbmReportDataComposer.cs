using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Models;
using DBM.Parser.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;

namespace CakeExtracter.Etl.DBM.Extractors.Composer
{
    /// <summary>DBM report data composer.</summary>
    //internal class DbmReportDataComposer<T> where T: DbmBaseReportRow
    internal class DbmReportDataComposer
    {
        public IEnumerable<DbmCreativeSummary> ComposeCreativeReportData(DbmAccountReportData reportData)
        {
            return reportData.Data.Select(row => new DbmCreativeSummary
            {
                Creative = new DbmCreative
                {
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
                    },
                    ExternalId = row.CreativeId,
                    Name = row.CreativeName,
                    Height = row.CreativeHeight,
                    Width = row.CreativeWidth,
                    Size = row.CreativeSize,
                    Type = row.CreativeType
                },
                Date = row.Date,
                Cost = row.Revenue,
                Impressions = row.Impressions,
                Clicks = row.Clicks,
                PostClickConv = row.PostClickConv,
                PostViewConv = row.PostViewConv,
                CMPostClickRevenue = row.CMPostClickRevenue,
                CMPostViewRevenue = row.CMPostViewRevenue
            });
        }

        //private IEnumerable<DbmCreativeSummary> CreateCreativeSummary(IGrouping<string, DbmCreativeReportRow> group)
        //{
        //    var externalId = group.Key;
        //    var list = group.ToList();
        //    return list.Select(row => new DbmCreativeSummary
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
        //                            Account = new ExtAccount
        //                            {
        //                                ExternalId = externalId
        //                            },
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
        //        Impressions = Convert.ToInt32(row.Impressions),
        //        Clicks = Convert.ToInt32(row.Clicks),
        //        PostClickConv = Convert.ToInt32(row.PostClickConv),
        //        PostViewConv = Convert.ToInt32(row.PostViewConv),
        //        CMPostClickRevenue = row.CMPostClickRevenue,
        //        CMPostViewRevenue = row.CMPostViewRevenue
        //    });
        //}


        /// <summary>Composes the report data.Group by accounts and report dimensions.</summary>
        /// <param name = "reportEntries" > The report entries.</param>
        /// <param name = "accounts" > The accounts.</param>
        /// <returns></returns>
        //public List<IGrouping<string, T>> ComposeReportData(IEnumerable<T> reportEntries, IEnumerable<ExtAccount> accounts)
        //{
        //    //var accountsData = reportEntries.GroupBy(e => e.AdvertiserId);

        //    //var relatedAccountsData = new List<IGrouping<string, T>>();
        //    //foreach (var account in accounts)
        //    //{
        //    //    var relatedAccountData = accountsData.Where(g => g.Key == account.ExternalId);
        //    //    relatedAccountsData.AddRange(relatedAccountData);
        //    //}

        //    //return relatedAccountsData;

        //    //var accountsData = reportEntries.GroupBy(re => re.AdvertiserId, (key, gr) =>
        //    //{
        //    //    var relatedAccount = accounts.FirstOrDefault(ac => ac.ExternalId == key);
        //    //    return relatedAccount != null ? new DbmAccountReportData
        //    //    {
        //    //        Account = relatedAccount,
        //    //        DailyDataCollection = GetAccountReportData(gr)
        //    //    } : null;
        //    //});
        //    //return accountsData.Where(data => data != null).ToList(); ;
        //}

        //private List<DbmDailyReportData> GetAccountReportData(IEnumerable<DbmBaseReportRow> reportEntries)
        //{
        //    return reportEntries.GroupBy(row => row.Date, (key, gr) => new DbmDailyReportData
        //    {
        //        Date = key,
        //        Creatives = GetCreatives(gr),
        //        LineItems = GetLineItemsFromCreatives(gr),
        //        InsertionOrders = GetOrdersFromCreatives(gr),
        //        Advertisers = GetAdvertisersFromCreatives(gr)
        //    }).ToList();
        //}

        //private List<DbmCreative> GetCreatives(IEnumerable<DbmBaseReportRow> creatives)
        //{
        //    return creatives.Select(row => new DbmCreative
        //    {
        //        Name = row.CreativeName,
        //        ReportId = row.CreativeId,
        //        Advertiser = row.AdvertiserName,
        //        AdvertiserReportId = row.AdvertiserId,
        //        Order = row.OrderName,
        //        OrderReportId = row.OrderId,
        //        LineItem = row.LineItemName,
        //        LineItemReportId = row.LineItemId,
        //        //Metrics
        //        TotalCost = row.TotalCost,
        //        Impressions = row.Impressions,
        //        ClickThroughs = row.ClickThroughs,
        //        TotalPixelEvents = row.TotalPixelEvents,
        //        TotalPixelEventsViews = row.TotalPixelEventsViews,
        //        TotalPixelEventsClicks = row.TotalPixelEventsClicks,
        //        DPV = row.DPV,
        //        ATC = row.ATC,
        //        Purchase = row.Purchase,
        //        PurchaseViews = row.PurchaseViews,
        //        PurchaseClicks = row.PurchaseClicks
        //    }
        //    ).ToList();
        //}
    }
}
