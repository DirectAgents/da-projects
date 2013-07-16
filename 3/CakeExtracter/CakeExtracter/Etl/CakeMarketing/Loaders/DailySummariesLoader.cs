using System;
using System.Collections.Generic;
using System.Data;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class DailySummariesLoader : Loader<OfferDailySummary>
    {
        protected override int Load(List<OfferDailySummary> items)
        {
            var loaded = 0;
            var added = 0;
            var updated = 0;
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                foreach (var item in items)
                {
                    var source = item.DailySummary;
                    var pk1 = item.OfferId;
                    var pk2 = DateTime.Parse(source.Date);

                    // BUG: this doesn't update, does it?
                    var target = db.Set<ClientPortal.Data.Contexts.DailySummary>().Find(pk1, pk2)
                                 ?? new ClientPortal.Data.Contexts.DailySummary
                                     {
                                         offer_id = pk1,
                                         date = pk2,
                                         views = source.Views,
                                         clicks = source.Clicks,
                                         click_thru = source.ClickThru,
                                         conversions = source.Conversions,
                                         paid = source.Paid,
                                         sellable = source.Sellable,
                                         conversion_rate = source.ConversionRate,
                                         cpl = source.CPL,
                                         cost = source.Cost,
                                         rpt = source.RPT,
                                         revenue = source.Revenue,
                                         margin = source.Margin,
                                         profit = source.Profit,
                                         epc = source.EPC
                                     };

                    if (db.Entry(target).State == EntityState.Detached)
                    {
                        db.DailySummaries.Add(target);
                        added++;
                    }
                    else
                    {
                        updated++;
                    }
                    loaded++;
                }
                Logger.Info("Loading {0} DailySummaries ({1} updates, {2} additions)", loaded, updated, added);
                db.SaveChanges();
            }
            return loaded;
        }
    }
}