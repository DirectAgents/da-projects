using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.Loaders;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class DbmConvLoader : Loader<DataTransferRow>
    {
        private DbmConvConverter convConverter;

        public DbmConvLoader(DbmConvConverter convConverter)
        {
            this.convConverter = convConverter;
        }

        protected override int Load(List<DataTransferRow> items)
        {
            var convs = items.Select(i => CreateConv(i)).ToList();
            var count = UpsertConversions(convs);
            return count;
        }

        private Conv CreateConv(DataTransferRow dtRow)
        {
            var conv = new Conv
            {
                //AccountId =
                Time = convConverter.EventTime(dtRow)
                //ConvType =
                //ConvVal =
                //ExtData =
                //IP =
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
    }
}
