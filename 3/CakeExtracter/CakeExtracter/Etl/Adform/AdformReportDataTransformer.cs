using System;
using System.Collections.Generic;
using Adform.Entities;
using Adform.Entities.ReportEntities;

namespace CakeExtracter.Etl.Adform
{
    public class AdformReportDataTransformer
    {
        private readonly ReportData reportData;
        private readonly List<List<object>> rows;
        private readonly Dictionary<string, int> columnLookup;
        private readonly bool includeBasicStats;
        private readonly bool includeConvStats;
        private readonly bool includeAdInteractionType;
        private readonly bool includeCampaign;
        private readonly bool includeOrder;
        private readonly bool includeLineItem;
        private readonly bool includeTrackingPoint;
        private readonly bool includeBanner;

        public AdformReportDataTransformer(
            ReportData reportData,
            bool basicStatsOnly = false,
            bool convStatsOnly = false,
            bool byCampaign = false,
            bool byLineItem = false,
            bool byBanner = false,
            bool byOrder = false,
            bool byTrackingPoint = false,
            bool filterInteractionType = false)
        {
            this.reportData = reportData;
            rows = reportData.rows;
            columnLookup = reportData.CreateColumnLookup();
            includeBasicStats = !convStatsOnly;
            includeConvStats = !basicStatsOnly;
            includeAdInteractionType = filterInteractionType ? false : !basicStatsOnly;
            includeCampaign = byCampaign;
            includeOrder = byOrder;
            includeLineItem = byLineItem;
            includeBanner = byBanner;
            includeTrackingPoint = byTrackingPoint;
        }

        public IEnumerable<AdformReportSummary> EnumerateAdformSummaries()
        {
            foreach (var row in rows)
            {
                var summary = RowToAdformSummary(row);
                if (summary != null)
                {
                    yield return summary;
                }
            }
        }

        private AdformReportSummary RowToAdformSummary(List<object> row)
        {
            try
            {
                var summary = new AdformReportSummary();
                AssignSummaryProperties(summary, row);
                return summary;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        private void AssignSummaryProperties(AdformReportSummary summary, List<object> row)
        {
            summary.Date = DateTime.Parse(row[columnLookup[reportData.DateColumnId]].ToString());
            summary.MediaId = row[columnLookup[reportData.MediaIdColumnId]].ToString();
            summary.Media = ReturnValueIfColumnExists(row, reportData.MediaColumnId, Convert.ToString);
            if (includeCampaign)
            {
                summary.Campaign = ReturnValueIfColumnExists(row, reportData.CampaignColumnId, Convert.ToString);
                summary.CampaignId = ReturnValueIfColumnExists(row, reportData.CampaignIdColumnId, Convert.ToString);
            }
            if (includeOrder)
            {
                summary.Order = ReturnValueIfColumnExists(row, reportData.OrderColumnId, Convert.ToString);
                summary.OrderId = ReturnValueIfColumnExists(row, reportData.OrderIdColumnId, Convert.ToString);
            }
            if (includeLineItem)
            {
                summary.LineItem = ReturnValueIfColumnExists(row, reportData.LineItemColumnId, Convert.ToString);
                summary.LineItemId = ReturnValueIfColumnExists(row, reportData.LineItemIdColumnId, Convert.ToString);
            }
            if (includeTrackingPoint)
            {
                summary.TrackingPoint = ReturnValueIfColumnExists(row, reportData.TrackingPointColumnId, Convert.ToString);
                summary.TrackingPointId = ReturnValueIfColumnExists(row, reportData.TrackingPointIdColumnId, Convert.ToString);
            }
            if (includeBanner)
            {
                summary.Banner = ReturnValueIfColumnExists(row, reportData.BannerColumnId, Convert.ToString);
                summary.BannerId = ReturnValueIfColumnExists(row, reportData.BannerIdColumnId, Convert.ToString);
            }
            if (includeAdInteractionType)
            {
                AssignAdInteractionType(summary, row);
            }
            if (includeBasicStats)
            {
                AssignBasicSummaryProperties(summary, row);
            }
            if (includeConvStats)
            {
                AssignConversionSummaryProperties(summary, row);
            }
        }

        private void AssignAdInteractionType(AdformReportSummary summary, List<object> row)
        {
            summary.AdInteractionType = ReturnValueIfColumnExists(row, reportData.AdInteractionColumnId, Convert.ToString);
            if (summary.AdInteractionType.StartsWith("Recent ")) // "Recent Impresson", "Recent Click": strip off "Recent "
            {
                summary.AdInteractionType = summary.AdInteractionType.Substring(7);
            }
        }

        private void AssignBasicSummaryProperties(AdformReportSummary summary, List<object> row)
        {
            summary.Cost = ReturnValueIfColumnExists(row, reportData.CostColumnId, Convert.ToDecimal);
            summary.Impressions = ReturnValueIfColumnExists(row, reportData.ImpressionColumnId, Convert.ToInt32);
            summary.Clicks = ReturnValueIfColumnExists(row, reportData.ClickColumnId, Convert.ToInt32);
        }

        private void AssignConversionSummaryProperties(AdformReportSummary summary, List<object> row)
        {
            AssignConversionProperties(summary, row);
            AssignSaleProperties(summary, row);
            AssignUniqueImpressionsProperty(summary, row);
        }

        private void AssignConversionProperties(AdformReportSummary summary, List<object> row)
        {
            summary.ConversionsAll = ReturnValueIfColumnExists(row, reportData.ConversionAllColumnId, Convert.ToInt32);
            summary.ConversionsConvType1 = ReturnValueIfColumnExists(row, reportData.Conversion1ColumnId, Convert.ToInt32);
            summary.ConversionsConvType2 = ReturnValueIfColumnExists(row, reportData.Conversion2ColumnId, Convert.ToInt32);
            summary.ConversionsConvType3 = ReturnValueIfColumnExists(row, reportData.Conversion3ColumnId, Convert.ToInt32);
        }

        private void AssignSaleProperties(AdformReportSummary summary, List<object> row)
        {
            summary.SalesAll = ReturnValueIfColumnExists(row, reportData.SalesAllColumnId, Convert.ToDecimal);
            summary.SalesConvType1 = ReturnValueIfColumnExists(row, reportData.Sales1ColumnId, Convert.ToDecimal);
            summary.SalesConvType2 = ReturnValueIfColumnExists(row, reportData.Sales2ColumnId, Convert.ToDecimal);
            summary.SalesConvType3 = ReturnValueIfColumnExists(row, reportData.Sales3ColumnId, Convert.ToDecimal);
        }

        private void AssignUniqueImpressionsProperty(AdformReportSummary summary, List<object> row)
        {
            summary.UniqueImpressions = ReturnValueIfColumnExists(row, reportData.UniqueImpressionsColumnId, Convert.ToInt32);
        }

        private T ReturnValueIfColumnExists<T>(List<object> row, string columnId, Func<object, T> convertFunc)
        {
            return columnLookup.ContainsKey(columnId)
                ? convertFunc(row[columnLookup[columnId]])
                : default(T);
        }
    }
}
