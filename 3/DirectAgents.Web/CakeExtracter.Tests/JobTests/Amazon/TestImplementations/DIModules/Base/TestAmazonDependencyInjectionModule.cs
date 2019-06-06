using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.CPProg;
using Moq;
using Ninject.Activation;
using Ninject.Modules;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base
{
    internal class TestAmazonDependencyInjectionModule : NinjectModule
    {
        public override void Load()
        {
            BindExtractors();
            BindLoaders();
            BindServices();
            BindRepositories();
        }

        protected virtual void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtracter> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() }).Verifiable();
        }

        protected virtual void SetupAdExtractor(Mock<AmazonApiAdExtrator> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
        }

        protected virtual void SetupKeywordExtractor(Mock<AmazonApiKeywordExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
        }

        protected virtual void SetupDailyLoader(Mock<AmazonDailySummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Verifiable();
        }

        protected virtual void SetupAdLoader(Mock<AmazonAdSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<TDadSummary>>())).Verifiable();
        }

        protected virtual void SetupKeywordLoader(Mock<AmazonKeywordSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Verifiable();
        }

        private void BindExtractors()
        {
            Bind<AmazonDatabaseKeywordsToDailySummaryExtracter>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonDatabaseKeywordsToDailySummaryExtracter>(context, SetupDailyExtractor));
            Bind<AmazonApiAdExtrator>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonApiAdExtrator>(context, SetupAdExtractor));
            Bind<AmazonApiKeywordExtractor>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonApiKeywordExtractor>(context, SetupKeywordExtractor));
        }

        private void BindLoaders()
        {
            Bind<BaseAmazonLevelLoader<DailySummary, DailySummaryMetric>>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonDailySummaryLoader>(context, SetupDailyLoader));
            Bind<BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonAdSummaryLoader>(context, SetupAdLoader));
            Bind<BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonKeywordSummaryLoader>(context, SetupKeywordLoader));
        }

        private void BindServices()
        {
            Bind<IJobExecutionItemService>().To<JobExecutionItemService>();
            Bind<IJobExecutionRequestScheduler>().To<JobExecutionRequestScheduler>();
        }

        private void BindRepositories()
        {
            Bind<IBaseRepository<JobRequestExecution>>().To<TestJobExecutionItemRepository>();
            Bind<IBaseRepository<JobRequest>>().To<TestJobRequestRepository>().InSingletonScope();
        }

        private T GetMockObjectWithParameterizedConstructor<T>(IContext context, Action<Mock<T>> setupMockAction)
            where T : class
        {
            var constructorArgs = context.Parameters.Select(x => x.GetValue(context, null)).ToArray();
            var extractorMock = new Mock<T>(constructorArgs)
            {
                CallBase = true,
            };
            setupMockAction(extractorMock);
            return extractorMock.Object;
        }
    }
}
