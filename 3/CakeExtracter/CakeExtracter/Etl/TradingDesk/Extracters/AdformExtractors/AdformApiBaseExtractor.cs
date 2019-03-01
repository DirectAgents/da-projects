using System;
using System.Collections.Generic;
using Adform;
using Adform.Entities;
using Adform.Enums;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.Extracters.AdformExtractors
{
    public abstract class AdformApiBaseExtractor<T> : Extracter<T>
        where T : DatedStatsSummary
    {
        protected readonly AdformUtility AfUtility;
        protected readonly DateRange DateRange;
        protected readonly int ClientId;
        protected Dictionary<DateTime, decimal> MonthlyCostMultipliers = new Dictionary<DateTime, decimal>();

        protected AdformApiBaseExtractor(AdformUtility adformUtility, DateRange dateRange, ExtAccount account)
        {
            AfUtility = adformUtility;
            DateRange = dateRange;
            ClientId = int.Parse(account.ExternalId);

            // Starting 2/1/18, Adform pulls in "client cost" rather than "DA cost" so we have to convert back to "client cost"
            var specialConversionStart = new DateTime(2018, 2, 1);
            if (account.Campaign == null)
            {
                return;
            }
            var firstOfMonth = new DateTime(dateRange.FromDate.Year, dateRange.FromDate.Month, 1);
            while (firstOfMonth <= dateRange.ToDate)
            {
                if (firstOfMonth >= specialConversionStart)
                {
                    var pbi = account.Campaign.PlatformBudgetInfoFor(firstOfMonth, account.PlatformId, useParentValsIfNone: true);
                    MonthlyCostMultipliers[firstOfMonth] = pbi.MediaSpendToRevMultiplier * pbi.RevToCostMultiplier;
                }
                firstOfMonth = firstOfMonth.AddMonths(1);
            }
        }

        protected ReportSettings GetBaseSettings()
        {
            return new ReportSettings
            {
                StartDate = DateRange.FromDate,
                EndDate = DateRange.ToDate,
                ClientId = ClientId,
                BasicMetrics = true,
                ConvMetrics = true,
                RtbOnly = true,
                Dimensions = new List<Dimension> { Dimension.AdInteractionType}
            };
        }

        protected IEnumerable<T> AdjustItems(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var monthStart = new DateTime(item.Date.Year, item.Date.Month, 1);
                if (MonthlyCostMultipliers.ContainsKey(monthStart))
                {
                    item.Cost *= MonthlyCostMultipliers[monthStart];
                }

                yield return item;
            }
        }
    }
}
