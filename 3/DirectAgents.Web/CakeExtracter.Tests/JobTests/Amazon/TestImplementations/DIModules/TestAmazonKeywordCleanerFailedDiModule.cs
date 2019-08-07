using System;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading bindings for amazon tests where there is an exception in keyword cleaner.
    /// </summary>
    internal class TestAmazonKeywordCleanerFailedDiModule : TestAmazonDependencyInjectionModule
    {
        /// <inheritdoc />
        protected override void SetupKeywordExtractor(Mock<AmazonApiKeywordExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Throws<Exception>().Verifiable();
        }
    }
}
