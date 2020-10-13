using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.Criteo.Exceptions;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using Criteo;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class CriteoApiExtracter<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly string AccountCode;
        protected readonly DateRange DateRange;
        protected readonly int TimezoneOffset;
        protected readonly int AccountId;

        protected CriteoUtility CriteoUtility;
        //TODO: make it so that within the criteoUtility, it doesn't need to open and close the service for every call

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<CriteoFailedEtlException> ProcessFailedExtraction;

        protected CriteoApiExtracter(CriteoUtility criteoUtility, string accountCode, int accountId, DateRange dateRange, int timezoneOffset = 0)
        {
            AccountId = accountId;
            AccountCode = accountCode;
            DateRange = dateRange;
            TimezoneOffset = timezoneOffset;
            if (criteoUtility != null)
            {
                CriteoUtility = criteoUtility;
            }
            else
            {
                CriteoUtility = new CriteoUtility(m => Logger.Info(m), m => Logger.Warn(m));
                CriteoUtility.SetCredentials(accountCode);
            }
        }

        protected IEnumerable<StrategySummary> EnumerateXmlReportRows(string reportUrl)
        {
            var rows = CriteoApiExtracter.EnumerateCriteoXmlReportRows(reportUrl);
            foreach (var row in rows)
            {
                var sum = new StrategySummary
                {
                    StrategyEid = row["campaignID"],
                    Date = DateTime.Parse(row["dateTime"]).AddHours(TimezoneOffset).Date,
                    Cost = decimal.Parse(row["cost"]),
                    Impressions = int.Parse(row["impressions"]),
                    Clicks = int.Parse(row["click"]),
                    PostClickConv = int.Parse(row["sales"]),
                    PostClickRev = decimal.Parse(row["orderValue"]),
                };
                yield return sum;
            }
        }

        protected void ProcessFailedStatsExtraction(Exception e, DateTime fromDate, DateTime toDate, int accountId, string statsType)
        {
            Logger.Error(e);
            var exception = new CriteoFailedEtlException(fromDate, toDate, accountId, e, statsType);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
