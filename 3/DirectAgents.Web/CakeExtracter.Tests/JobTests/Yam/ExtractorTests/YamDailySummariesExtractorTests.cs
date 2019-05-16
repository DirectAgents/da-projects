using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.YAM.Extractors.ApiExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CakeExtracter.Tests.JobTests.Yam.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Moq;
using NUnit.Framework;
using Yahoo;
using Yahoo.Models;

namespace CakeExtracter.Tests.JobTests.Yam.ExtractorTests
{
    [TestFixture(TestName = "YAM Daily Summaries extractor tests.")]
    [Category("Jobs")]
    [Description("Test proper behaviour of YAM DailySummaries extractor.")]
    public class YamDailySummariesExtractorTests
    {
        private const double MaxDecimal = 79228162514264337593543955.0;

        private readonly ExtAccount testAccount = PredefinedTestObjectsStorage.YamTestAccount;
        private readonly DateRange testDateRange = PredefinedTestObjectsStorage.YamTestDateRange;

        private Mock<YamUtility> utilityMock;
        private Mock<YamCsvExtractor> csvExtractorMock;

        [OneTimeSetUp]
        public void Initialize()
        {
            utilityMock = new Mock<YamUtility>();
            csvExtractorMock = new Mock<YamCsvExtractor>(testAccount);
            utilityMock.Setup(m => m.TryGenerateReport(It.IsAny<ReportSettings>())).Returns(PredefinedTestObjectsStorage.YamReportUrl);
        }

        [Test(Description = "Checks correct not-empty items transformation (without pixel parameter metrics) from API entities to entities that are ready for a loader.")]
        [TestCase("2019/1/1", 1, 1, 1, 1, 1, 1)]
        [TestCase("2019/1/1", 1, 0, 0, 0, 0, 0)]
        [TestCase("2019/1/1", 0, 0, 0, 0, 0.5, 0)]
        [TestCase("2017/1/1", 8, 9, 13, 17888, 5.19, 18.3122)]
        [TestCase("2019/4/1", int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, MaxDecimal, MaxDecimal)]
        public void YamDailyExtractor_CheckCorrectItemsTransformationWithoutPixelParameter(string date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend)
        {
            csvExtractorMock.Setup(m => m.EnumerateRows(It.IsAny<string>())).Returns(new List<YamRow>
            {
                PredefinedTestObjectsStorage.CreateRow(DateTime.Parse(date), impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, null),
                PredefinedTestObjectsStorage.CreateRow(DateTime.Parse(date).AddDays(1), impressions, clicks,
                    conversionClicks, conversionViews, conversionValue, spend, null)
            });
            var summaries = GetTransformedSummaries(false);

            Assert.IsTrue(summaries.Count() == 2);
            Assert.IsTrue(summaries.All(x =>
                !x.ClickConversionValueByPixelQuery.HasValue 
                && !x.ViewConversionValueByPixelQuery.HasValue));
            Assert.IsTrue(summaries.All(x =>
                x.Impressions == impressions 
                && x.Clicks == clicks 
                && x.ClickThroughConversion == conversionClicks 
                && x.ViewThroughConversion == conversionViews 
                && x.ConversionValue == conversionValue 
                && x.AdvertiserSpending == spend));
        }

        [Test(Description = "Checks correct not-empty items transformation (with pixel parameter metrics) from API entities to entities that are ready for a loader.")]
        [TestCase("2019/1/1", 1, 1, 1, 1, 1, 1, "gv=18")]
        [TestCase("2019/1/1", 1, 0, 0, 0, 0, 0, "gv=0")]
        [TestCase("2019/1/1", 0, 0, 0, 0, 0.1, 0, "gv=0")]
        [TestCase("2019/1/1", 0, 0, 0, 1, 0, 0, "gv=18")]
        [TestCase("2017/1/1", 843, 9, 13, 345, 65.19, 128.3176522, "gv=18.5&&w=90")]
        [TestCase("2019/4/1", int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, MaxDecimal, MaxDecimal, "test=2&gv=14363453,85&w=90")]
        public void YamDailyExtractor_CheckCorrectItemsTransformationWithPixelParameter(string date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, string pixelParameter)
        {
            csvExtractorMock.Setup(m => m.EnumerateRows(It.IsAny<string>())).Returns(new List<YamRow>
            {
                PredefinedTestObjectsStorage.CreateRow(DateTime.Parse(date), impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, pixelParameter),
                PredefinedTestObjectsStorage.CreateRow(DateTime.Parse(date).AddDays(1), impressions, clicks,
                    conversionClicks, conversionViews, conversionValue, spend, pixelParameter)
            });
            var summaries = GetTransformedSummaries(true);

            Assert.IsTrue(summaries.Count() == 2);
            Assert.IsTrue(summaries.All(x =>
                x.ClickConversionValueByPixelQuery.HasValue
                && x.ViewConversionValueByPixelQuery.HasValue));
            Assert.IsTrue(summaries.All(x =>
                x.Impressions == impressions
                && x.Clicks == clicks
                && x.ClickThroughConversion == conversionClicks
                && x.ViewThroughConversion == conversionViews
                && x.ConversionValue == conversionValue
                && x.AdvertiserSpending == spend
                && (x.ClickThroughConversion > 0 ? pixelParameter.Contains(x.ClickConversionValueByPixelQuery.Value.ToString()) : true)
                && (x.ViewThroughConversion > 0 ? pixelParameter.Contains(x.ViewConversionValueByPixelQuery.Value.ToString()) : true)));
        }

        [Test(Description = "Checks API entities grouping to entities that are ready for a loader.")]
        [TestCase(2, "2019/1/1", 1, 1, 1, 1, 1, 1, "gv=18")]
        [TestCase(20, "2019/1/1", 1, 0, 0, 0, 0, 0, "gv=0")]
        [TestCase(4, "2019/1/1", 0, 0, 0, 0, 0.1, 0, "gv=0")]
        [TestCase(1000, "2019/1/1", 0, 0, 0, 1, 0, 0, "test=2&gv=14363453,85&w=90")]
        [TestCase(200, "2017/1/1", 843, 9, 13, 345, 65.19, 128.3176522, "gv=18.5&&w=90")]
        public void YamDailyExtractor_CheckItemsGrouping(int repeatingNum, string date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, string pixelParameter)
        {
            var realDate = DateTime.Parse(date);
            var row = PredefinedTestObjectsStorage.CreateRow(realDate, impressions, clicks, conversionClicks,
                conversionViews, conversionValue, spend, pixelParameter);
            var rows = Enumerable.Repeat(row, repeatingNum).ToList();
            csvExtractorMock.Setup(m => m.EnumerateRows(It.IsAny<string>())).Returns(rows);
            var summaries = GetTransformedSummaries(true);

            Assert.IsTrue(summaries.Count() == 1);
            Assert.IsTrue(summaries.All(x =>
                x.Date == realDate
                && x.Impressions == impressions * repeatingNum
                && x.Clicks == clicks * repeatingNum
                && x.ClickThroughConversion == conversionClicks * repeatingNum
                && x.ViewThroughConversion == conversionViews * repeatingNum
                && x.ConversionValue == conversionValue * repeatingNum
                && x.AdvertiserSpending == spend * repeatingNum
                && (x.ClickThroughConversion > 0 ? pixelParameter.Contains((x.ClickConversionValueByPixelQuery.Value / repeatingNum).ToString()) : true)
                && (x.ViewThroughConversion > 0 ? pixelParameter.Contains((x.ViewConversionValueByPixelQuery.Value / repeatingNum).ToString()) : true)));
        }

        [Test(Description = "Checks correct items transformation, empty items should be not returned.")]
        [TestCase(0, 0, 0, 0, 0, 0, "gv=0")]
        [TestCase(0, 0, 0, 0, 0, 0, "gv=18")]
        public void YamDailyExtractor_CheckEmptyItemsNotReturning(int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, string pixelParameter)
        {
            csvExtractorMock.Setup(m => m.EnumerateRows(It.IsAny<string>())).Returns(new List<YamRow>
            {
                PredefinedTestObjectsStorage.CreateRow(DateTime.Now, impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, pixelParameter)
            });
            var summaries1 = GetTransformedSummaries(true);

            Assert.IsEmpty(summaries1);

            const int defaultMetricValue = 1;
            csvExtractorMock.Setup(m => m.EnumerateRows(It.IsAny<string>())).Returns(new List<YamRow>
            {
                PredefinedTestObjectsStorage.CreateRow(DateTime.Now, impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, pixelParameter),
                PredefinedTestObjectsStorage.CreateRow(DateTime.Now, impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, pixelParameter),
                PredefinedTestObjectsStorage.CreateRow(DateTime.Now, defaultMetricValue, defaultMetricValue,
                    defaultMetricValue, defaultMetricValue, defaultMetricValue, defaultMetricValue, $"gv={defaultMetricValue}")
            });
            var summaries2 = GetTransformedSummaries(true);
            
            Assert.IsTrue(summaries2.Count() == 1);
            Assert.IsTrue(summaries2.All(x =>
                    x.Impressions == defaultMetricValue
                    && x.Clicks == defaultMetricValue
                    && x.ClickThroughConversion == defaultMetricValue
                    && x.ViewThroughConversion == defaultMetricValue
                    && x.ConversionValue == defaultMetricValue
                    && x.AdvertiserSpending == defaultMetricValue
                    && x.ClickConversionValueByPixelQuery == defaultMetricValue
                    && x.ViewConversionValueByPixelQuery == defaultMetricValue));
        }

        private IEnumerable<YamDailySummary> GetTransformedSummaries(bool byPixelParameter)
        {
            var extractor = new YamDailySummaryExtractor(csvExtractorMock.Object, utilityMock.Object, testDateRange,
                testAccount, byPixelParameter);
            return extractor.GetItemsFromApi();
        }
    }
}
