using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeExtracter.Etl.Criteo.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Loaders
{
    public class CriteoLoaders : TDStrategySummaryLoader
    {
        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<CriteoFailedEtlException> ProcessFailedLoading;

        protected override int Load(List<StrategySummary> items)
        {
            try
            {
                //throw new Exception();
                Logger.Info(AccountId, "Loading {0} DA-TD StrategySummaries..", items.Count);
                PrepareData(items);
                AddUpdateDependentStrategies(items);
                AssignStrategyIdToItems(items);
                var count = UpsertDailySummaries(items);
                return count;
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
            var exception = new CriteoFailedEtlException(fromDateArg, toDateArg, AccountId, e);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
