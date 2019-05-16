using System;
using System.Collections.Generic;
using CakeExtracter.Etl.YAM.Loaders;
using CakeExtracter.Etl.YAM.Repositories.Summaries;
using CakeExtracter.Tests.JobTests.Yam.Helpers;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using Moq;
using NUnit.Framework;

namespace CakeExtracter.Tests.JobTests.Yam.LoaderTests
{
    [TestFixture(TestName = "YAM Daily Summaries loader tests.")]
    [Category("Jobs")]
    [Description("Test proper behaviour of YAM DailySummaries loader.")]
    public class YamDailySummariesLoaderTests
    {
        private readonly ExtAccount testAccount = PredefinedTestObjectsStorage.YamTestAccount;

        private Mock<YamDailySummaryDatabaseRepository> summaryRepositoryMock;

        [OneTimeSetUp]
        public void Initialize()
        {
            summaryRepositoryMock = new Mock<YamDailySummaryDatabaseRepository>();
            summaryRepositoryMock.Setup(m => m.MergeItems(It.IsAny<List<YamDailySummary>>())).Returns(true);
        }

        [Test(Description = "Fills account ids in items for merge.")]
        [TestCase("1/1/2019", 1, 1, 1, 1, 1, 1, 1, 1)]
        [TestCase("1/1/2019", 1, 1, 1, 1, 1, 1, null, null)]
        public void YamDailyLoader_FillAccountIdsInItemsForMerge(string date, int impressions, int clicks, int conversionClicks,
            int conversionViews, decimal conversionValue, decimal spend, decimal? conversionValueClicksQuery,
            decimal? conversionValueViewsQuery)
        {
            var dataToLoad = new List<YamDailySummary>
            {
                PredefinedTestObjectsStorage.CreateSummary<YamDailySummary>(DateTime.Parse(date), impressions, clicks, conversionClicks,
                    conversionViews, conversionValue, spend, conversionValueClicksQuery, conversionValueViewsQuery)
            };
            var mergeResult = MergeItemsUsingLoader(dataToLoad);
            Assert.IsTrue(mergeResult);
            Assert.IsTrue(dataToLoad.TrueForAll(x => x.EntityId == testAccount.Id));
        }

        private bool MergeItemsUsingLoader(List<YamDailySummary> dataToLoad)
        {
            var loader = new YamDailySummaryLoader(testAccount.Id, summaryRepositoryMock.Object);
            var mergeResult = loader.MergeItemsWithExisted(dataToLoad);
            return mergeResult;
        }
    }
}
