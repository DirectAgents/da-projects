using System;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    class TestAmazonKeywordCleanerFailedDiModule : TestAmazonDependencyInjectionModule
    {
        protected override void SetupKeywordExtractor(Mock<AmazonApiKeywordExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Throws<Exception>().Verifiable();
        }
    }
}
