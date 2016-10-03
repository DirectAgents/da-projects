using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;
using System;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class DbmConvLoader : Loader<DataTransferRow>
    {
        private DbmConvConverter convConverter;
        private Dictionary<int, int> accountIdLookupByExtId = new Dictionary<int, int>();

        public DbmConvLoader(DbmConvConverter convConverter)
        {
            this.convConverter = convConverter;
        }

        protected override int Load(List<DataTransferRow> items)
        {
            UpdateAccountLookup(items);
            var convs = items.Select(i => CreateConv(i)).Where(i => i.AccountId > 0).ToList();
            var count = UpsertConversions(convs);
            return count;
        }

        private Conv CreateConv(DataTransferRow dtRow)
        {
            var conv = new Conv
            {
                AccountId = accountIdLookupByExtId[dtRow.insertion_order_id.Value],
                Time = convConverter.EventTime(dtRow),
                ConvType = (dtRow.event_sub_type == "postview") ? "v" : "c",
                //ConvVal =
                //ExtData =
                IP = dtRow.ip,
                
                //StrategyId, TDadId, CityId
            };
            return conv;
        }

        // "UpsertConvs" ?
        private int UpsertConversions(List<Conv> convs)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new DATDContext())
            {
                foreach (var conv in convs)
                {
                    itemCount++;
                }
                Logger.Info("Saving {0} Conv's ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void UpdateAccountLookup(List<DataTransferRow> items)
        {
            var acctExtIds = items.Select(i => i.insertion_order_id.Value).Distinct();

            using (var db = new DATDContext())
            {
                foreach (var acctExtId in acctExtIds)
                {
                    if (accountIdLookupByExtId.ContainsKey(acctExtId))
                        continue; // already encountered

                    var tdAccts = db.ExtAccounts.Where(a => a.ExternalId == acctExtId.ToString());
                    if (tdAccts.Count() == 1)
                    {
                        var tdAcct = tdAccts.First();
                        accountIdLookupByExtId[acctExtId] = tdAcct.Id;
                    }
                    else
                        accountIdLookupByExtId[acctExtId] = -1; //tag ExtAccounts that don't have Insertion_Order_Ids
                }
            }
        }
    }
}
