using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CommissionJunction.Enums;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.CJ;

namespace CakeExtracter.Etl.TradingDesk.Extracters.CommissionJunctionExtractors
{
    internal class CommissionJunctionAdvertiserExtractor : Extracter<CjAdvertiserCommission>
    {
        private readonly CjUtility utility;
        private readonly DateRangeType dateRangeType;
        private readonly DateRange dateRange;
        private readonly ExtAccount account;
        private readonly CommissionJunctionAdvertiserCleaner cleaner;
        private readonly CommissionJunctionAdvertiserMapper mapper;

        public CommissionJunctionAdvertiserExtractor(CjUtility utility, CommissionJunctionAdvertiserCleaner cleaner,
            DateRangeType dateRangeType, DateRange dateRange, ExtAccount account)
        {
            this.utility = utility;
            this.dateRangeType = dateRangeType;
            this.dateRange = dateRange;
            this.account = account;
            this.cleaner = cleaner;
            mapper = new CommissionJunctionAdvertiserMapper();
        }

        protected override void Extract()
        {
            Logger.Info(account.Id, "Extracting Commissions from Commission Junction API for ({0}) from {1:d} to {2:d} (date range type - {3})",
                account.ExternalId, dateRange.FromDate, dateRange.ToDate, dateRangeType);
            Extract(dateRange.FromDate, dateRange.ToDate);
            End();
        }

        private void Extract(DateTime fromDate, DateTime toDate)
        {
            var isCleaned = false;
            var commissionsEnumerable = utility.GetAdvertiserCommissions(dateRangeType, fromDate, toDate.AddDays(1), account.ExternalId);
            foreach (var commissions in commissionsEnumerable)
            {
                try
                {
                    CleanDataIfNeed(fromDate, toDate, ref isCleaned);
                    var items = mapper.MapCommissionJunctionInfoToDbEntities(commissions, account.Id);
                    Add(items);
                }
                catch (Exception e)
                {
                    Logger.Error(account.Id, e);
                }
            }
        }

        private void CleanDataIfNeed(DateTime fromDate, DateTime toDate, ref bool isCleaned)
        {
            if (isCleaned)
            {
                return;
            }

            cleaner.CleanCommissionJunctionInfo(account.Id, fromDate, toDate);
            isCleaned = true;
        }
    }
}
