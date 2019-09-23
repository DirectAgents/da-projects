using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading bindings for amazon tests where there is an exception in daily extractor.
    /// </summary>
    internal class TestAmazonDailyExtractorFailedDiModule : TestAmazonDependencyInjectionModule
    {
        /// <inheritdoc />
        protected override void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Throws<Exception>().Verifiable();
        }
    }
}
