﻿using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DSP.Extractors.Parser.Models;
using CakeExtracter.Etl.DSP.Models;

namespace CakeExtracter.Etl.DSP.Extractors.Composer
{
    internal class DspReportDataComposer
    {
        public List<AmazonDspDailyReportData> ComposeReportData(List<CreativeReportRow> reportEntries)
        {
            return GetReportData(reportEntries);
        }

        private List<AmazonDspDailyReportData> GetReportData(IEnumerable<CreativeReportRow> reportEntries)
        {
            return reportEntries.GroupBy(row => row.Date, (key, gr) => new AmazonDspDailyReportData
            {
                Date = key,
                Creatives = GetCreatives(gr),
                LineItems = GetLineItemsFromCreatives(gr),
                Orders = GetOrdersFromCreatives(gr),
                Advertisers = GetAdvertisersFromCreatives(gr)
            }).ToList();
        }

        private List<ReportCreative> GetCreatives(IEnumerable<CreativeReportRow> creatives)
        {
            return creatives.Select(row => new ReportCreative
            {
                Advertiser = row.AdvertiserName,
                Order = row.OrderName,
                LineItem = row.LineItemName,
                Name = row.CreativeName,
                TotalCost = row.TotalCost,
                Impressions = row.Impressions,
                ClickThroughs = row.ClickThroughs,
                TotalPixelEvents = row.TotalPixelEvents,
                TotalPixelEventsViews = row.TotalPixelEventsViews,
                TotalPixelEventsClicks = row.TotalPixelEventsClicks,
                DPV = row.DPV,
                ATC = row.ATC,
                Purchase = row.Purchase,
                PurchaseViews = row.PurchaseViews,
                PurchaseClicks = row.PurchaseClicks
            }
            ).ToList();
        }

        private List<ReportLineItem> GetLineItemsFromCreatives(IEnumerable<CreativeReportRow> creatives)
        {
            return creatives.GroupBy(creative => creative.LineItemName, (key, gr) =>
                {
                    var firstCreative = gr.FirstOrDefault();
                    var advertiserName = firstCreative != null && !string.IsNullOrEmpty(firstCreative.AdvertiserName)
                        ? firstCreative.AdvertiserName
                        : null;
                    var orderName = firstCreative != null && !string.IsNullOrEmpty(firstCreative.OrderName)
                        ? firstCreative.OrderName
                        : null;
                    return new ReportLineItem
                    {
                        Advertiser = advertiserName,
                        Order = orderName,
                        Name = key,
                        TotalCost = gr.Sum(creative => creative.TotalCost),
                        Impressions = gr.Sum(creative => creative.Impressions),
                        ClickThroughs = gr.Sum(creative => creative.ClickThroughs),
                        TotalPixelEvents = gr.Sum(creative => creative.TotalPixelEvents),
                        TotalPixelEventsViews = gr.Sum(creative => creative.TotalPixelEventsViews),
                        TotalPixelEventsClicks = gr.Sum(creative => creative.TotalPixelEventsClicks),
                        DPV = gr.Sum(creative => creative.DPV),
                        ATC = gr.Sum(creative => creative.ATC),
                        Purchase = gr.Sum(creative => creative.Purchase),
                        PurchaseViews = gr.Sum(creative => creative.PurchaseViews),
                        PurchaseClicks = gr.Sum(creative => creative.PurchaseClicks)
                    };
                }
            ).ToList();
        }

        private List<ReportOrder> GetOrdersFromCreatives(IEnumerable<CreativeReportRow> creatives)
        {
            return creatives.GroupBy(creative => creative.OrderName, (key, gr) =>
                {
                    var firstCreative = gr.FirstOrDefault();
                    var advertiserName = firstCreative != null && !string.IsNullOrEmpty(firstCreative.AdvertiserName)
                        ? firstCreative.AdvertiserName
                        : null;
                    return new ReportOrder
                    {
                        Advertiser = advertiserName,
                        Name = key,
                        TotalCost = gr.Sum(creative => creative.TotalCost),
                        Impressions = gr.Sum(creative => creative.Impressions),
                        ClickThroughs = gr.Sum(creative => creative.ClickThroughs),
                        TotalPixelEvents = gr.Sum(creative => creative.TotalPixelEvents),
                        TotalPixelEventsViews = gr.Sum(creative => creative.TotalPixelEventsViews),
                        TotalPixelEventsClicks = gr.Sum(creative => creative.TotalPixelEventsClicks),
                        DPV = gr.Sum(creative => creative.DPV),
                        ATC = gr.Sum(creative => creative.ATC),
                        Purchase = gr.Sum(creative => creative.Purchase),
                        PurchaseViews = gr.Sum(creative => creative.PurchaseViews),
                        PurchaseClicks = gr.Sum(creative => creative.PurchaseClicks)
                    };
                }
            ).ToList();
        }

        private List<ReportAdvertiser> GetAdvertisersFromCreatives(IEnumerable<CreativeReportRow> creatives)
        {
            return creatives.GroupBy(creative => creative.AdvertiserName, (key, gr) =>
                new ReportAdvertiser
                {
                    Name = key,
                    TotalCost = gr.Sum(creative => creative.TotalCost),
                    Impressions = gr.Sum(creative => creative.Impressions),
                    ClickThroughs = gr.Sum(creative => creative.ClickThroughs),
                    TotalPixelEvents = gr.Sum(creative => creative.TotalPixelEvents),
                    TotalPixelEventsViews = gr.Sum(creative => creative.TotalPixelEventsViews),
                    TotalPixelEventsClicks = gr.Sum(creative => creative.TotalPixelEventsClicks),
                    DPV = gr.Sum(creative => creative.DPV),
                    ATC = gr.Sum(creative => creative.ATC),
                    Purchase = gr.Sum(creative => creative.Purchase),
                    PurchaseViews = gr.Sum(creative => creative.PurchaseViews),
                    PurchaseClicks = gr.Sum(creative => creative.PurchaseClicks)
                }
            ).ToList();
        }
    }
}
