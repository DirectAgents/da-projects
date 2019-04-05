using CakeExtracter.SimpleRepositories.BasicRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// The repository for working with Job Request objects in the database.
    /// </summary>
    internal class JobRequestRepository : BasicDatabaseRepository<JobRequest, ClientPortalProgContext>
    {
        /// <inheritdoc />
        public override string EntityName => "Job Request";

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of <see cref="JobRequestRepository"/>
        /// </summary>
        /// <param name="locker">The object to lock work with Job Request entities in a database.</param>
        public JobRequestRepository(object locker) : base(locker)
        {
        }

        /// <inheritdoc />
        public override object[] GetKeys(JobRequest item)
        {
            return new object[] {item.Id};
        }
    }
}
