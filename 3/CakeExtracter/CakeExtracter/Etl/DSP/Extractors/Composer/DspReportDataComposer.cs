using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DSP.Extractors.Parser.Models;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DSP.Extractors.Composer
{
    internal class DspReportDataComposer
    {
        public List<AmazonDspAccauntReportData> ComposeReportData(List<CreativeReportRow> reportEntries, List<ExtAccount> accounts)
        {
            var accountsData = reportEntries.GroupBy(re => re.AdvertiserName, (key, gr) =>
             {
                 var relatedAccount = accounts.FirstOrDefault(ac => ac.Name == key);
                 return relatedAccount != null ? new AmazonDspAccauntReportData
                 {
                     Account = relatedAccount,
                     DailyDataCollection = GetAccountReportData(gr)
                 } : null;
             });
            return accountsData.Where(data => data != null).ToList(); ;
        }

        private List<AmazonDspDailyReportData> GetAccountReportData(IEnumerable<CreativeReportRow> reportEntries)
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
                Name = row.CreativeName,
                ReportId = row.CreativeId,
                Advertiser = row.AdvertiserName,
                AdvertiserReportId = row.AdvertiserId,
                Order = row.OrderName,
                OrderReportId = row.OrderId,
                LineItem = row.LineItemName,
                LineItemReportId = row.LineItemId,
                //Metrics
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
                    var firstCreative = gr.First();
                    return new ReportLineItem
                    {
                        Name = key,
                        ReportId = firstCreative.LineItemId,
                        Advertiser = firstCreative.AdvertiserName,
                        AdvertiserReportId = firstCreative.AdvertiserId,
                        Order = firstCreative.OrderName,
                        OrderReportId = gr.First().OrderId,
                        //Metrics
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
                    var firstCreative = gr.First();
                    return new ReportOrder
                    {
                        Name = key,
                        ReportId = firstCreative.OrderId,
                        Advertiser = firstCreative.AdvertiserName,
                        AdvertiserReportId = firstCreative.AdvertiserId,
                        //Metrics
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
            return creatives.GroupBy(creative => creative.AdvertiserName, (key, gr) => {
                var firstCreative = gr.First();
                return new ReportAdvertiser
                {
                    Name = key,
                    ReportId = firstCreative.AdvertiserId,
                    //Metrics
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
            }).ToList();
        }
    }
}
