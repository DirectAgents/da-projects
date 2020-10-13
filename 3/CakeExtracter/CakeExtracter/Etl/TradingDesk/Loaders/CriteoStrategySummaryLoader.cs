using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Etl.Criteo.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class CriteoStrategySummaryLoader : TDStrategySummaryLoader
    {
        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<CriteoFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="CriteoStrategySummaryLoader"/> class.
        /// </summary>
        /// <param name="AccountId"></param>
        public CriteoStrategySummaryLoader(int AccountId = -1)
            : base(AccountId)
        {
        }

        /// <inheritdoc/>
        protected override int Load(List<StrategySummary> items)
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

        private void ProcessFailedStatsLoading(Exception e, List<StrategySummary> items)
        {
            Logger.Error(e);
            var fromDate = items.Min(x => x.Date);
            var toDate = items.Max(x => x.Date);
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new CriteoFailedEtlException(fromDateArg, toDateArg, AccountId, e, StatsTypeAgg.StrategyArg);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
