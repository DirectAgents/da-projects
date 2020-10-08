using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Etl.Criteo.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class CriteoDailySummaryLoader : TDDailySummaryLoader
    {
        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<CriteoFailedEtlException> ProcessFailedLoading;

        public CriteoDailySummaryLoader( int accountId = -1)
            : base(accountId)
        {
        }

        protected override int Load(List<DailySummary> items)
        {
            try
            {
                return base.Load(items);
            }
            catch (Exception e)
            {
                ProcessFailedStatsLoading(e, items);
                return items.Count;
            }
        }

        private void ProcessFailedStatsLoading(Exception e, List<DailySummary> items)
        {
            Logger.Error(e);
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new CriteoFailedEtlException(fromDateArg, toDateArg, accountId, e, StatsTypeAgg.DailyArg);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
