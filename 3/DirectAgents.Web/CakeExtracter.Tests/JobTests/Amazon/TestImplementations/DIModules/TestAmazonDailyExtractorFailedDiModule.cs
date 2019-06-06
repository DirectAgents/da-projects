using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    class TestAmazonDailyExtractorFailedDiModule : TestAmazonDependencyInjectionModule
    {
        protected override void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtracter> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Throws<Exception>().Verifiable();
        }
    }
}
