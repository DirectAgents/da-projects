using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// The repository for working with Job Request objects in the database.
    /// </summary>
    internal class JobRequestRepository : BaseDatabaseRepository<JobRequest, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Job Request";

        /// <inheritdoc />
        public override object[] GetKeys(JobRequest item)
        {
            return new object[] {item.Id};
        }
    }
}
