using System;
using System.Data.SqlTypes;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
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
        private readonly CommissionJunctionAdvertiserCleaner cleaner;

        public CommissionJunctionAdvertiserExtractor(CjUtility utility, CommissionJunctionAdvertiserCleaner cleaner, DateRange dateRange, ExtAccount account)
        {
            this.utility = utility;
            this.dateRange = dateRange;
            this.account = account;
            this.cleaner = cleaner;
        }

        protected override void Extract()
        {
            Logger.Info(account.Id, "Extracting Commissions from Commission Junction API for ({0}) from {1:d} to {2:d}",
                account.ExternalId, dateRange.FromDate, dateRange.ToDate);
            //TODO: Invoke when it's right place with correct params
            //cleaner.CleanCommissionJunctionInfo(account.Id, (DateTime)SqlDateTime.MinValue, (DateTime)SqlDateTime.MaxValue); 
            var commissionsEnumerable = utility.GetAdvertiserCommissions(dateRange.FromDate, dateRange.ToDate, account.ExternalId);
            foreach (var  commission in commissionsEnumerable)
            {
                try
                {
                    Add(new CjAdvertiserCommission());
                }
                catch (Exception e)
                {
                    Logger.Error(account.Id, e);
                }
            }

            End();
        }
    }
}
