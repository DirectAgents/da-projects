using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using DirectAgents.Domain.Entities.CPProg;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    class TestAmazonDailyCleanerFailedDiModule : TestAmazonDependencyInjectionModule
    {
        protected override void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtracter> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Throws<Exception>().Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() });
        }
    }
}
