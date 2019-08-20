using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers.Interfaces;
using CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors;
using CakeExtracter.Etl.Amazon.Loaders;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.CPProg;
using Ninject.Modules;

namespace CakeExtracter.Bootstrappers
{
    /// <inheritdoc />
    /// <summary>
    /// DI module for loading app bindings.
    /// </summary>
    internal class DependencyInjectionModule : NinjectModule
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

        private void BindExtractors()
        {
            Bind<AmazonDatabaseKeywordsToDailySummaryExtractor>().To<AmazonDatabaseKeywordsToDailySummaryExtractor>();
            Bind<AmazonApiAdExtractor>().To<AmazonApiAdExtractor>();
            Bind<AmazonApiKeywordExtractor>().To<AmazonApiKeywordExtractor>();
        }

        private void BindLoaders()
        {
            Bind<BaseAmazonLevelLoader<DailySummary, DailySummaryMetric>>().To<AmazonDailySummaryLoader>();
            Bind<BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>>().To<AmazonAdSummaryLoader>();
            Bind<BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>>().To<AmazonKeywordSummaryLoader>();
            Bind<BaseAmazonLevelLoader<SearchTermSummary, SearchTermSummaryMetric>>().To<AmazonSearchTermSummaryLoader>();
        }

        private void BindServices()
        {
            Bind<IJobExecutionItemService>().To<JobExecutionItemService>().InSingletonScope();
            Bind<IJobRequestLifeCycleManager>().To<JobRequestLifeCycleManager>().InSingletonScope();
            Bind<IJobExecutionRequestLauncher>().To<JobExecutionRequestLauncher>().InSingletonScope();
            Bind<IJobExecutionNotificationService>().To<JobExecutionNotificationService>().InSingletonScope();
            Bind<IEmailNotificationsService>().To<EmailNotificationsService>().InSingletonScope();
            Bind<IProcessManager>().To<ProcessManager>().InSingletonScope();
        }

        private void BindRepositories()
        {
            Bind<IPlatformAccountRepository>().To<PlatformAccountRepository>().InSingletonScope();
            Bind<IBaseRepository<JobRequestExecution>>().To<JobExecutionItemRepository>().InSingletonScope();
            Bind<IJobRequestsRepository>().To<JobRequestRepository>().InSingletonScope();
        }
    }
}
