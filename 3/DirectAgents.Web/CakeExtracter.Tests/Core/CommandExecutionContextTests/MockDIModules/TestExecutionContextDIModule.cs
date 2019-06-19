using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers.Interfaces;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Ninject.Modules;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests.MockDIModules
{
    internal class TestExecutionContextDIModule : NinjectModule
    {
        private readonly IBaseRepository<JobRequestExecution> jobRequestExecutionRepository;

        private readonly IJobRequestsRepository jobRequestsRepository;

        public TestExecutionContextDIModule(
            IBaseRepository<JobRequestExecution> jobRequestExecutionRepository,
            IJobRequestsRepository jobRequestsRepository)
        {
            this.jobRequestExecutionRepository = jobRequestExecutionRepository;
            this.jobRequestsRepository = jobRequestsRepository;
        }

        public override void Load()
        {
            Bind<IJobExecutionItemService>().To<JobExecutionItemService>();
            Bind<IJobRequestLifeCycleManager>().To<JobRequestLifeCycleManager>();

            Bind<IBaseRepository<JobRequestExecution>>().ToMethod((context) => { return jobRequestExecutionRepository; });
            Bind<IJobRequestsRepository>().ToMethod((context) => { return jobRequestsRepository; });
        }
    }
}
