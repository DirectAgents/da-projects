﻿using System;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CommissionJunction.Enums;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.CJ;
using System.Data.SqlTypes;

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
            //TODO: Invoke when it's right place with correct params
            cleaner.CleanCommissionJunctionInfo(account.Id, (DateTime)SqlDateTime.MinValue, (DateTime)SqlDateTime.MaxValue); 
            var commissionsEnumerable = utility.GetAdvertiserCommissions(dateRangeType, dateRange.FromDate, dateRange.ToDate, account.ExternalId);
            foreach (var commissions in commissionsEnumerable)
            {
                try
                {
                    var items = mapper.MapCommissionJunctionInfoToDbEntities(commissions.ToList(), account.Id);
                    Add(items);
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
