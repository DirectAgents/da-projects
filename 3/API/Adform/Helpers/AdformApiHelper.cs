using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Adform.Entities;
using Adform.Entities.ReportEntities;
using Adform.Enums;

namespace Adform.Helpers
{
    internal class AdformApiHelper
    {
        private const string DateFormat = "yyyy'-'M'-'d";
        private const string RtbName = "Real Time Bidding";

        private const string CostMetric = "cost";
        private const string ImpressionsMetric = "impressions";
        private const string ClicksMetric = "clicks";
        private const string ConversionsMetric = "conversions";
        private const string SalesMetric = "sales";

        private static readonly Dictionary<Dimension, string> Dimensions = new Dictionary<Dimension, string>
        {
            {Dimension.Date, "date"},
            {Dimension.Campaign, "campaign"},
            {Dimension.Order, "order"},
            {Dimension.LineItem, "lineItem"},
            {Dimension.Banner, "banner"},
            {Dimension.Media, "media"},
            {Dimension.AdInteractionType, "adInteractionType"} // Click, Impression, etc.
        };

        public static ExpandoObject GetFilters(ReportSettings settings)
        {
            dynamic filter = new ExpandoObject();
            filter.client = new[] {settings.ClientId};
            if (settings.TrackingId != null)
            {
                filter.tracking = settings.TrackingId;
            }

            filter.date = new Dates
            {
                from = settings.StartDate.ToString(DateFormat),
                to = settings.EndDate.ToString(DateFormat)
            };
            if (settings.RtbOnly)
            {
                filter.media = new {name = new[] {RtbName}};
            }

            return filter;
        }

        public static string[] GetMetrics(ReportSettings settings)
        {
            var metrics = new List<string>();
            if (settings.BasicMetrics)
            {
                metrics.AddRange(new[] {CostMetric, ImpressionsMetric, ClicksMetric});

            }

            if (settings.ConvMetrics)
            {
                metrics.AddRange(new[] {ConversionsMetric, SalesMetric});
            }

            return metrics.ToArray();
        }

        public static string[] GetDimensions(ReportSettings settings)
        {
            var defaultDimensions = new[] {Dimension.Date};
            var dimensions = settings.Dimensions == null
                ? defaultDimensions
                : settings.Dimensions.Concat(defaultDimensions);
            var strDimensions = dimensions.Select(x => Dimensions[x]).ToArray();
            return strDimensions;
        }
    }
}
