using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CommissionJunction.Enums;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.CJ;

namespace CakeExtracter.Etl.TradingDesk.Extracters.CommissionJunctionExtractors
{
    /// <summary>
    /// Commission Junction advertiser extractor.
    /// </summary>
    internal class CommissionJunctionAdvertiserExtractor : Extracter<CjAdvertiserCommission>
    {
        private readonly CjUtility utility;
        private readonly DateRangeType dateRangeType;
        private readonly DateRange dateRange;
        private readonly ExtAccount account;
        private readonly CommissionJunctionAdvertiserCleaner cleaner;
        private readonly CommissionJunctionAdvertiserMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommissionJunctionAdvertiserExtractor"/> class.
        /// </summary>
        /// <param name="utility">Utility to extract items for a target account <see cref="CjUtility"/>.</param>
        /// <param name="cleaner">Commission Junction Info cleaner <see cref="CommissionJunctionAdvertiserCleaner"/></param>
        /// <param name="dateRangeType">Date range filter type <see cref="DateRangeType"/></param>
        /// <param name="dateRange">Date range.</param>
        /// <param name="account">Database account.</param>
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

        /// <summary>
        /// Extract specified items.
        /// </summary>
        protected override void Extract()
        {
            Logger.Info(account.Id, "Extracting Commissions from Commission Junction API for ({0}) from {1:d} to {2:d} (date range type - {3})",
                account.ExternalId, dateRange.FromDate, dateRange.ToDate, dateRangeType);
            Extract(dateRange.FromDate, dateRange.ToDate);
            End();
        }

        private void Extract(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var commissions = utility.GetAdvertiserCommissions(dateRangeType, fromDate, toDate.AddDays(1), account.ExternalId);
                var items = mapper.MapCommissionJunctionInfoToDbEntities(commissions, account.Id);
                cleaner.CleanCommissionJunctionInfo(account.Id, fromDate, toDate);
                Add(items);
            }
            catch (Exception e)
            {
                Logger.Error(account.Id, e);
            }
        }
    }
}
