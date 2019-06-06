using System;
using System.Collections.Generic;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using DirectAgents.Domain.Entities.CPProg;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    class TestAmazonKeywordLoaderFailedDiModule : TestAmazonDependencyInjectionModule
    {
        protected override void SetupKeywordLoader(Mock<AmazonKeywordSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Throws<Exception>().Verifiable();
        }
    }
}
