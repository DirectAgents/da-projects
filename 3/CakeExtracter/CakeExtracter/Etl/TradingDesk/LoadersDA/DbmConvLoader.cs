using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
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
            var count = TDConvLoader.UpsertConvs(convs);
            return count;
        }

        private Conv CreateConv(DataTransferRow dtRow)
        {
            var conv = new Conv
            {
                AccountId = accountIdLookupByExtId[dtRow.insertion_order_id.Value],
                Time = convConverter.EventTime(dtRow),
                ConvType = (dtRow.event_sub_type == "postview") ? "v" : "c",
                //ConvVal = 0,
                IP = dtRow.ip
                //StrategyId, TDadId, CityId
            };
            return conv;
        }

        private void UpdateAccountLookup(List<DataTransferRow> items)
        {
            var acctExtIds = items.Select(i => i.insertion_order_id.Value).Distinct();

            using (var db = new ClientPortalProgContext())
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
                    {
                        var newAccount = new ExtAccount
                        {
                            ExternalId = acctExtId.ToString(),
                            PlatformId = Platform.GetId(db,Platform.Code_DBM),
                            Name = "Unknown"
                        };
                        db.ExtAccounts.Add(newAccount);
                        db.SaveChanges();
                        Logger.Info("Added new ExtAccount: InsertionOrder {1}", acctExtId);
                        accountIdLookupByExtId[acctExtId] = newAccount.Id;
                    }
                }
            }
        }
    }
}
