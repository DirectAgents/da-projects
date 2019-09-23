using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// Job Execution Item Repository.
    /// </summary>
    /// <seealso cref="T:CakeExtracter.SimpleRepositories.BaseRepositories.BaseDatabaseRepository`2" />
    public class JobExecutionItemRepository : BaseDatabaseRepository<JobRequestExecution, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        public override string EntityName => "Job Execution Item Request";

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override object[] GetKeys(JobRequestExecution item)
        {
            return new object[] { item.Id };
        }
    }
}
