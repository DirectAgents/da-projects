using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA.CommissionJunctionLoaders;
using CommissionJunction.Enums;
using CommissionJunction.Exceptions;
using CommissionJunction.Utilities;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.CJ;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

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
        private readonly List<DateTime> datesForCleaning;
        private readonly ExtAccount account;
        private readonly CommissionJunctionAdvertiserCleaner cleaner;
        private readonly CommissionJunctionAdvertiserMapper mapper;

        public event Action<FailedEtlException> ProcessFailedExtraction;

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
            datesForCleaning = dateRange.Dates.ToList();
            mapper = new CommissionJunctionAdvertiserMapper();
            ProcessFailedExtraction += exception => Logger.Error(account.Id, exception);
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
                var items = GetCommissions(fromDate, toDate);
                CleanCommissions();
                Add(items);
            }
            catch (Exception e)
            {
                var exception = new FailedEtlException(fromDate, toDate, account.Id, e);
                ProcessFailedExtraction?.Invoke(exception);
            }
        }

        private IEnumerable<CjAdvertiserCommission> GetCommissions(DateTime fromDate, DateTime toDate)
        {
            utility.ProcessSkippedCommissions += ProcessSkippedCommissions;
            var commissions = utility.GetAdvertiserCommissions(dateRangeType, fromDate, toDate.AddDays(1), account.ExternalId);
            utility.ProcessSkippedCommissions -= ProcessSkippedCommissions;
            var items = mapper.MapCommissionJunctionInfoToDbEntities(commissions, account.Id);
            return items;
        }

        private void ProcessSkippedCommissions(SkippedCommissionsException exception)
        {
            var excDateRange = new DateRange(exception.StartDate, exception.EndDate);
            excDateRange.Dates.ForEach(x => datesForCleaning.Remove(x));
        }

        private void CleanCommissions()
        {
            if (datesForCleaning.Count == 0)
            {
                return;
            }

            var fromDateIndex = 0;
            var toDateIndex = fromDateIndex;
            for (var i = fromDateIndex + 1; i < datesForCleaning.Count; i++)
            {
                if (datesForCleaning[toDateIndex].AddDays(1) == datesForCleaning[i])
                {
                    toDateIndex = i;
                }
                else
                {
                    CleanCommissions(datesForCleaning[fromDateIndex], datesForCleaning[toDateIndex]);
                    fromDateIndex = toDateIndex = i;
                }
            }

            CleanCommissions(datesForCleaning[fromDateIndex], datesForCleaning[toDateIndex]);
        }

        private void CleanCommissions(DateTime fromDate, DateTime toDate)
        {
            var nextDayAfterToDate = toDate.AddDays(1); // cleaning non including toDate
            cleaner.CleanCommissionJunctionInfo(account.Id, dateRangeType, fromDate, nextDayAfterToDate);
        }
    }
}
