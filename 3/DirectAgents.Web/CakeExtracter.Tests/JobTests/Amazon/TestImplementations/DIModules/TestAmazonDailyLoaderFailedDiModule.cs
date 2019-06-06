using System;
using System.Collections.Generic;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base;
using DirectAgents.Domain.Entities.CPProg;
using Moq;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules
{
    class TestAmazonDailyLoaderFailedDiModule : TestAmazonDependencyInjectionModule
    {
        protected override void SetupDailyLoader(Mock<AmazonDailySummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Throws<Exception>().Verifiable();
        }
    }
}
