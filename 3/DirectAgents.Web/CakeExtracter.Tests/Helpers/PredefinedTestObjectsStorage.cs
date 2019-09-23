using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Tests.Helpers
{
    internal static class PredefinedTestObjectsStorage
    {
        public const string ReportUrl = "TestURL";

        public static DateRange TestDateRange => new DateRange(new DateTime(2019, 1, 1), new DateTime(2019, 1, 31));

        public static ExtAccount TestAccount1 => new ExtAccount
        {
            Id = 765,
            Name = "TestAccount1",
            ExternalId = "1234",
        };

        public static ExtAccount TestAccount2 => new ExtAccount
        {
            Id = 789,
            Name = "TestAccount2",
            ExternalId = "5689",
        };

        public static ExtAccount TestAccount3 => new ExtAccount
        {
            Id = 78129,
            Name = "TestAccount3",
            ExternalId = "44444",
        };

        public static T CreateYamSummary<T>(DateTime date, int impressions, int clicks, int conversionClicks,
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

        public static YamRow CreateYamRow(DateTime date, int impressions, int clicks, int conversionClicks,
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
