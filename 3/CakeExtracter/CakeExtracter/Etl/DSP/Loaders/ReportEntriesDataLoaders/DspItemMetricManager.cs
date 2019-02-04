using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    /// <summary>Processes metrics related operations.</summary>
    internal class DspItemMetricManager
    {
        /// <summary>Gets the metric values entity from report entity.</summary>
        /// <typeparam name="TMetricValues">The type of the metric values.</typeparam>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="dbEntityId">The database entity identifier.</param>
        /// <param name="date">The metrics date.</param>
        /// <returns></returns>
        public TMetricValues GetMetricValuesEntities<TMetricValues>(DspReportEntity reportEntity, int dbEntityId, DateTime date)
            where TMetricValues : DspMetricValues, new()
        {
            var metricValues = new TMetricValues();
            metricValues.TotalCost = reportEntity.TotalCost;
            metricValues.Impressions = reportEntity.Impressions;
            metricValues.ClickThroughs = reportEntity.ClickThroughs;
            metricValues.TotalPixelEvents = reportEntity.TotalPixelEvents;
            metricValues.TotalPixelEventsViews = reportEntity.TotalPixelEventsViews;
            metricValues.TotalPixelEventsClicks = reportEntity.TotalPixelEventsClicks;
            metricValues.DPV = reportEntity.DPV;
            metricValues.ATC = reportEntity.ATC;
            metricValues.Purchase = reportEntity.Purchase;
            metricValues.PurchaseViews = reportEntity.PurchaseViews;
            metricValues.PurchaseClicks = reportEntity.PurchaseClicks;
            metricValues.EntityId = dbEntityId;
            metricValues.Date = date;
            return AreAllMetricValuesNullable(metricValues) ? null : metricValues;
        }

        /// <summary>  Checkes whether the metric values are equivalent.</summary>
        /// <typeparam name="TMetricValues">The type of the metric values.</typeparam>
        /// <param name="existing">The existing metric values.</param>
        /// <param name="actual">The actual metric values.</param>
        /// <returns>Indicator whether metric values equivalent.</returns>
        public bool AreMetricValuesEquivalent<TMetricValues>(TMetricValues existing, TMetricValues actual)
            where TMetricValues : DspMetricValues
        {
            return existing.TotalCost == actual.TotalCost &&
                existing.Impressions == actual.Impressions &&
                existing.ClickThroughs == actual.ClickThroughs &&
                existing.TotalPixelEvents == actual.TotalPixelEvents &&
                existing.TotalPixelEventsViews == actual.TotalPixelEventsViews &&
                existing.TotalPixelEventsClicks == actual.TotalPixelEventsClicks &&
                existing.DPV == actual.DPV &&
                existing.ATC == actual.ATC &&
                existing.Purchase == actual.Purchase &&
                existing.PurchaseViews == actual.PurchaseViews &&
                existing.PurchaseClicks == actual.PurchaseClicks;
        }

        /// <summary>  Checkes whether all metric values are nullable.</summary>
        /// <typeparam name="TMetricValues">The type of the metric values.</typeparam>
        /// <param name="metricValues">The metric values.</param>
        /// <returns>Indicator whether all metric values are nullable.</returns>
        public bool AreAllMetricValuesNullable<TMetricValues>(TMetricValues metricValues)
            where TMetricValues : DspMetricValues
        {
            return metricValues.TotalCost == 0 &&
                metricValues.Impressions == 0 &&
                metricValues.ClickThroughs == 0 &&
                metricValues.TotalPixelEvents == 0 &&
                metricValues.TotalPixelEventsViews == 0 &&
                metricValues.TotalPixelEventsClicks == 0 &&
                metricValues.DPV == 0 &&
                metricValues.ATC == 0 &&
                metricValues.Purchase == 0 &&
                metricValues.PurchaseViews == 0 &&
                metricValues.PurchaseClicks == 0;
        }
    }
}
