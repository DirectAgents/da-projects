using System;
using System.Web;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformProviders.Implementation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Ninject.Web.Common.WebHost;
using CakeExtracter.Common.Email;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestLaunchers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestsLifeCycleManagers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;
using DirectAgents.Web.Areas.Admin.Grids.DataProviders;
using DirectAgents.Web.Areas.Admin.Grids.JobHistory;
using DirectAgents.Web.Areas.Admin.Grids.JobRequest;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DirectAgents.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(DirectAgents.Web.App_Start.NinjectWebCommon), "Stop")]

namespace DirectAgents.Web.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            var kernel = CreateKernel();
            bootstrapper.Initialize(() => kernel);
            DIKernel.SetKernel(kernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISecurityRepository>().To<SecurityRepository>();
            kernel.Bind<IMainRepository>().To<MainRepository>();

            kernel.Bind<ICPProgRepository>().To<CPProgRepository>();
            kernel.Bind<ICPSearchRepository>().To<CPSearchRepository>();
            kernel.Bind<IClientRepository>().To<ClientRepository>();

            kernel.Bind<ITDRepository>().To<TDRepository>();
            kernel.Bind<IRevTrackRepository>().To<RevTrackRepository>();
            kernel.Bind<IABRepository>().To<ABRepository>();
            kernel.Bind<ISuperRepository>().To<SuperRepository>();
            kernel.Bind<ISpecialPlatformRepository>().To<SpecialPlatformRepository>();

            kernel.Bind<IFacebookWebPortalDataService>().To<FacebookWebPortalDataService>();
            kernel.Bind<IPlatformAccountRepository>().To<PlatformAccountRepository>();

            kernel.Bind<IGridDataProvider<JobRequestExecution>>().To<JobHistoryDataProvider>();
            kernel.Bind<IGridDataProvider<JobRequest>>().To<JobRequestsDataProvider>();
            kernel.Bind<IJobRequestsRepository>().To<JobRequestRepository>();
            kernel.Bind<IBaseRepository<JobRequestExecution>>().To<JobExecutionItemRepository>();
            kernel.Bind<IJobExecutionItemService>().To<JobExecutionItemService>();
            kernel.Bind<IJobRequestLifeCycleManager>().To<JobRequestLifeCycleManager>();
            kernel.Bind<IJobExecutionNotificationService>().To<JobExecutionNotificationService>();
            kernel.Bind<IJobExecutionRequestLauncher>().To<JobExecutionRequestLauncher>();
            kernel.Bind<IEmailNotificationsService>().To<EmailNotificationsService>();
            kernel.Bind<IProcessManager>().To<ProcessManager>().InSingletonScope();
            kernel.Bind<ClientPortal.Data.Contracts.IClientPortalRepository>().To<ClientPortal.Data.Services.ClientPortalRepository>();

            kernel.Bind<SpecialPlatformProvider>().To<DspProvider>();
            kernel.Bind<SpecialPlatformProvider>().To<VcdProvider>();
            kernel.Bind<SpecialPlatformProvider>().To<KochavaProvider>();
            kernel.Bind<SpecialPlatformProvider>().To<CommissionJunctionProvider>();
        }
    }
}
