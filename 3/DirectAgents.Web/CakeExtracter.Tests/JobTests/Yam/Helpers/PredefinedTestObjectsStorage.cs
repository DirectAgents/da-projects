using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Tests.JobTests.Yam.Helpers
{
    internal static class PredefinedTestObjectsStorage
    {
        public const string YamReportUrl = "TestURL";

        public static DateRange YamTestDateRange => new DateRange(new DateTime(2019, 1, 1), new DateTime(2019, 1, 31));

        public static ExtAccount YamTestAccount => new ExtAccount
        {
            Id = 765,
            Name = "TestYamAccount",
            ExternalId = "1234",
        };

        public static T CreateSummary<T>(DateTime date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, decimal? conversionValueClicksQuery,
            decimal? conversionValueViewsQuery)
            where T : BaseYamSummary, new()
        {
            return new T
            {
                Date = date,
                Impressions = impressions,
                Clicks = clicks,
                ClickThroughConversion = conversionClicks,
                ViewThroughConversion = conversionViews,
                ConversionValue = conversionValue,
                AdvertiserSpending = spend,
                ClickConversionValueByPixelQuery = conversionValueClicksQuery,
                ViewConversionValueByPixelQuery = conversionValueViewsQuery
            };
        }

        public static YamRow CreateRow(DateTime date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, string pixelParameter)
        {
            return new YamRow
            {
                Date = date,
                Impressions = impressions,
                Clicks = clicks,
                ClickThroughConversion = conversionClicks,
                ViewThroughConversion = conversionViews,
                ConversionValue = conversionValue,
                AdvertiserSpending = spend,
                PixelParameter = pixelParameter
            };
        }
    }
}
