using System;
using System.Linq;
using CakeExtracter.Common;
using CommissionJunction.Entities;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.CJ;

namespace CakeExtracter.Etl.TradingDesk.Extracters.CommissionJunctionExtractors
{
    internal class CommissionJunctionAdvertiserExtractor : Extracter<CjAdvertiserCommission>
    {
        private readonly CjUtility utility;
        private readonly DateRange dateRange;
        private readonly ExtAccount account;

        public CommissionJunctionAdvertiserExtractor(CjUtility utility, DateRange dateRange, ExtAccount account)
        {
            this.utility = utility;
            this.dateRange = dateRange;
            this.account = account;
        }

        protected override void Extract()
        {
            Logger.Info(account.Id, "Extracting Commissions from Commission Junction API for ({0}) from {1:d} to {2:d}",
                account.ExternalId, dateRange.FromDate, dateRange.ToDate);

            var commissionsEnumerable = utility.GetAdvertiserCommissions(dateRange.FromDate, dateRange.ToDate, account.ExternalId);
            foreach (var  commissions in commissionsEnumerable)
            {
                try
                {
                    var items = commissions.Select(TransformCommission).ToList();
                    Add(items);
                }
                catch (Exception e)
                {
                    Logger.Error(account.Id, e);
                }
            }

            End();
        }

        private CjAdvertiserCommission TransformCommission(AdvertiserCommission commission)
        {
            return null;
        }
    }
}
