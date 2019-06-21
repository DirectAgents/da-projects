using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using DirectAgents.Domain.Entities.CPProg;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading bindings for amazon tests where there is an exception in daily cleaner.
    /// </summary>
    internal class TestAmazonDailyCleanerFailedDiModule : TestAmazonDependencyInjectionModule
    {
        /// <inheritdoc />
        protected override void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtracter> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Throws<Exception>().Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() });
        }
    }
}
