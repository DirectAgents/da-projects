using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Ninject.Modules;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.MockDIModules
{
    internal class TestExecutionContextDIModule : NinjectModule
    {
        private readonly IBaseRepository<JobRequestExecution> jobRequestExecutionRepository;
        private readonly IJobRequestsRepository jobRequestsRepository;
        private readonly IProcessManager processManager;

        public TestExecutionContextDIModule(
            IBaseRepository<JobRequestExecution> jobRequestExecutionRepository,
            IJobRequestsRepository jobRequestsRepository)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
            this.jobRequestsRepository = jobRequestsRepository;
        }

        public TestExecutionContextDIModule(
            IBaseRepository<JobRequestExecution> jobRequestExecutionRepository,
            IJobRequestsRepository jobRequestsRepository,
            IProcessManager processManager)
            : this(jobRequestExecutionRepository, jobRequestsRepository)
        {
            this.processManager = processManager;
        }

        public override void Load()
        {
            Bind<IJobExecutionItemService>().To<JobExecutionItemService>();
            Bind<IJobRequestLifeCycleManager>().To<JobRequestLifeCycleManager>();

            if (processManager == null)
            {
                Bind<IProcessManager>().To<ProcessManager>();
            }
            else
            {
                Bind<IProcessManager>().ToMethod(context => processManager);
            }
            Bind<IBaseRepository<JobRequestExecution>>().ToMethod(context => jobRequestExecutionRepository);
            Bind<IJobRequestsRepository>().ToMethod(context => jobRequestsRepository);
        }
    }
}
