using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// The repository for working with Job Request objects in the database.
    /// </summary>
    public class JobRequestRepository : BaseDatabaseRepository<JobRequest, ClientPortalProgContext>, IJobRequestsRepository
    {
        /// <inheritdoc />
        public override string EntityName => "Job Request";

        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override object[] GetKeys(JobRequest item)
        {
            return new object[] { item.Id };
        }

        /// <inheritdoc />
        public List<JobRequest> GetAllChildrenRequests(JobRequest jobRequest)
        {
            return GetAllChildrenRequestsRecursively(new List<JobRequest> { jobRequest });
        }

        private List<JobRequest> GetAllChildrenRequestsRecursively(List<JobRequest> parentJobRequests)
        {
            if (parentJobRequests?.Count > 0)
            {
                var parentRequestsIds = parentJobRequests.Select(jobRequest => jobRequest.Id).ToList();
                var levelChildren = GetItems(jobRequest => jobRequest.ParentJobRequestId != null
                    && parentRequestsIds.Contains(jobRequest.ParentJobRequestId.Value)).ToList();
                levelChildren.AddRange(GetAllChildrenRequestsRecursively(levelChildren));
            }
            return new List<JobRequest>();
        }
    }
}
