using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.CPProg;
using Moq;
using Ninject.Activation;
using Ninject.Modules;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations.DIModules.Base
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading amazon test bindings.
    /// </summary>
    internal class TestAmazonDependencyInjectionModule : NinjectModule
    {
        /// <inheritdoc />
        /// <summary>
        /// Loads bindings for specified services.
        /// </summary>
        public override void Load()
        {
            BindExtractors();
            BindLoaders();
            BindServices();
            BindRepositories();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon daily extractor.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon daily extractor.</param>
        protected virtual void SetupDailyExtractor(Mock<AmazonDatabaseKeywordsToDailySummaryExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
            extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() }).Verifiable();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon ad extractor.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon ad extractor.</param>
        protected virtual void SetupAdExtractor(Mock<AmazonApiAdExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon keyword extractor.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon keyword extractor.</param>
        protected virtual void SetupKeywordExtractor(Mock<AmazonApiKeywordExtractor> extractorMock)
        {
            extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon daily loader.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon daily loader.</param>
        protected virtual void SetupDailyLoader(Mock<AmazonDailySummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Verifiable();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon ad loader.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon ad loader.</param>
        protected virtual void SetupAdLoader(Mock<AmazonAdSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<TDadSummary>>())).Verifiable();
        }

        /// <summary>
        /// Specifies a setup on the mocked amazon keyword loader.
        /// </summary>
        /// <param name="extractorMock">Mock implementation of amazon keyword loader.</param>
        protected virtual void SetupKeywordLoader(Mock<AmazonKeywordSummaryLoader> loaderMock)
        {
            loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Verifiable();
        }

        private void BindExtractors()
        {
            Bind<AmazonDatabaseKeywordsToDailySummaryExtractor>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonDatabaseKeywordsToDailySummaryExtractor>(context, SetupDailyExtractor));
            Bind<AmazonApiAdExtractor>().ToMethod(context =>
                GetMockObjectWithParameterizedConstructor<AmazonApiAdExtractor>(context, SetupAdExtractor));
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
            Bind<IJobRequestLifeCycleManager>().To<JobRequestLifeCycleManager>();
        }

        private void BindRepositories()
        {
            Bind<IBaseRepository<JobRequestExecution>>().To<TestJobExecutionItemRepository>();
            Bind<IJobRequestsRepository>().To<TestJobRequestRepository>().InSingletonScope();
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
