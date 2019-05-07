using System.Collections.Generic;
using System.Linq;
using Yahoo.Constants;
using Yahoo.Constants.Enums;
using Yahoo.Models;

namespace Yahoo.Helpers
{
    internal static class ReportParametersHelper
    {
        private const string DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";

        public static ReportPayload CreateReportRequestPayload(ReportSettings reportSettings)
        {
            //This produced an InvalidTimeZoneException so we're just going with the system timezone, relying on it to be Eastern(daylight savings adjusted)
            //var offset = TimeZoneInfo.FindSystemTimeZoneById(@"Eastern Standard Time\Dynamic DST").BaseUtcOffset;
            //var start = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, offset);
            //var end = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, offset);
            var adjustedEndDate = reportSettings.ToDate.AddDays(1).AddSeconds(-1);
            var payload = new ReportPayload
            {
                reportOption = GetReportOption(reportSettings),
                intervalTypeId = (int) IntervalTypeId.Day,
                dateTypeId = (int) DateTypeId.CustomRange,
                startDate = reportSettings.FromDate.ToString(DateFormat),
                endDate = adjustedEndDate.ToString(DateFormat)
            };
            return payload;
        }

        private static ReportOption GetReportOption(ReportSettings reportSettings)
        {
            var reportOption = new ReportOption
            {
                timezone = Timezone.NewYork,
                currency = (int) Currency.Usd,
                accountIds = GetAccounts(reportSettings),
                dimensionTypeIds = GetDimensions(reportSettings),
                metricTypeIds = GetMetrics(reportSettings)
            };
            return reportOption;
        }

        private static int[] GetAccounts(ReportSettings reportSettings)
        {
            return reportSettings.AccountId.HasValue ? new[] {reportSettings.AccountId.Value} : new int[0];
        }

        private static int[] GetMetrics(ReportSettings reportSettings)
        {
            var metricList = reportSettings.ByPixelParameter && reportSettings.IsOutdated
                ? new[]
                {
                    // used to obtain the *real* conversion values from the pixel parameter
                    Metric.ClickThroughConversions, Metric.ViewThroughConversions
                }
                : new[]
                {
                    Metric.Impressions, Metric.Clicks, Metric.AdvertiserSpending, Metric.ClickThroughConversions,
                    Metric.ViewThroughConversions, Metric.RoasActionValue
                };
            return metricList.Select(x => (int)x).ToArray();
        }

        private static int[] GetDimensions(ReportSettings reportSettings)
        {
            var dimensionList = new List<int>();
            AddDimension(dimensionList, reportSettings.ByCampaign, Dimension.Campaign);
            AddDimension(dimensionList, reportSettings.ByLine, Dimension.Line);
            AddDimension(dimensionList, reportSettings.ByAd, Dimension.Ad);
            AddDimension(dimensionList, reportSettings.ByCreative, Dimension.Creative);
            AddDimension(dimensionList, reportSettings.ByPixel, Dimension.Pixel);
            AddDimension(dimensionList, reportSettings.ByPixelParameter, Dimension.PixelParameter);
            return dimensionList.ToArray();
        }

        private static void AddDimension(List<int> dimensionList, bool condition, Dimension dimension)
        {
            if (condition)
            {
                dimensionList.Add((int) dimension);
            }
        }
    }
}
