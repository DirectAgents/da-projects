using System.Collections.Generic;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories
{
    /// <summary>
    /// Job requests repository.
    /// </summary>
    /// <seealso cref="CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces.IBaseRepository{DirectAgents.Domain.Entities.Administration.JobExecution.JobRequest}" />
    public interface IJobRequestsRepository : IBaseRepository<JobRequest>
    {
        /// <summary>
        /// Gets all children of requests. Includes children of all levels(child of child etc.)
        /// </summary>
        /// <param name="jobRequest">The job request.</param>
        /// <returns>
        /// Collection of children requests.
        /// </returns>
        List<JobRequest> GetAllChildrenRequests(JobRequest jobRequest);
    }
}
