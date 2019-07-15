using System;
using System.Collections.Generic;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using DirectAgents.Domain.Entities.CPProg;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading bindings for amazon tests where there is an exception in keyword loader.
    /// </summary>
    internal class TestAmazonKeywordLoaderFailedDiModule : TestAmazonDependencyInjectionModule
    {
        /// <inheritdoc />
        protected override void SetupKeywordLoader(Mock<AmazonKeywordSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Throws<Exception>().Verifiable();
        }
    }
}
