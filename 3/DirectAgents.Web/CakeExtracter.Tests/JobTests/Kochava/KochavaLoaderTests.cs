using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Loaders;
using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Kochava;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CakeExtracter.Tests.JobTests.Kochava
{
    [TestFixture(TestName = "Kochava loader tests.")]
    [Category("Jobs")]
    [Description("Test proper behaviour of Kochava Loader.")]
    public class KochavaLoaderTests
    {
        private Mock<KochavaConfigurationProvider> configurationProviderMock;

        private Mock<KochavaItemsDbService> dbServiceMock;

        const int reportPeriodDays = 10;

        [OneTimeSetUp]
        public void Initialize()
        {
            configurationProviderMock = new Mock<KochavaConfigurationProvider>();
            dbServiceMock = new Mock<KochavaItemsDbService>();
            configurationProviderMock.Setup(m => m.GetReportPeriodInDays()).Returns(reportPeriodDays);
            dbServiceMock.Setup(m => m.BulkDeleteItems(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Verifiable();
            dbServiceMock.Setup(m => m.BulkInsertItems(It.IsAny<int>(), It.IsAny<List<KochavaItem>>())).Verifiable();
        }

        [Test(Description = "Invokes cleaner with correct date range.")]
        public void KochavaLoader_InvokesCleanerWithCorrectParams()
        {
            const int reportPeriodDays = 10;
            var kochavaLoader = new KochavaLoader(configurationProviderMock.Object, dbServiceMock.Object);
            var dataToLoad = new List<KochavaReportItem>()
            {
                new KochavaReportItem(),
                new KochavaReportItem()
            };
            var testAccount = new ExtAccount
            {
                Id = 765,
                Name = "TestKochavaAccount",
                ExternalId = "1234",
            };
            kochavaLoader.LoadData(dataToLoad, testAccount);
            dbServiceMock.Verify(dService => dService.BulkDeleteItems(
                It.Is<int>(p => p == testAccount.Id), 
                It.Is<DateTime>(p=>p == DateTime.Now.Date.AddDays(-reportPeriodDays)), 
                It.Is<DateTime>(p => p == DateTime.Now.Date)));
        }

        [Test(Description = "Filles account ids in items for insert.")]
        public void KochavaLoader_FillAccountIdsInItemsForInsert()
        {
            var kochavaLoader = new KochavaLoader(configurationProviderMock.Object, dbServiceMock.Object);
            var dataToLoad = new List<KochavaReportItem>()
            {
                new KochavaReportItem(),
                new KochavaReportItem()
            };
            var testAccount = new ExtAccount
            {
                Id = 765,
                Name = "TestKochavaAccount",
                ExternalId = "1234",
            };
            kochavaLoader.LoadData(dataToLoad, testAccount);
            dbServiceMock.Verify(dService => dService.BulkInsertItems(
                It.Is<int>(p => p == testAccount.Id),
                It.Is<List<KochavaItem>> (p => p.TrueForAll(i=>i.AccountId == testAccount.Id))));
        }
    }
}
