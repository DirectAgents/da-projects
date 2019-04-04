using CakeExtracter.Common.JobRequests.Repositories;
using CakeExtracter.SimpleRepositories.BasicRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Google.Apis.Storage.v1.Data;

namespace CakeExtracter.SimpleRepositories
{
    /// <summary>
    /// The class contains properties for obtaining singleton repositories.
    /// </summary>
    internal static class RepositoriesContainer
    {
        private static IBasicRepository<JobRequest> jobRequestRepository;

        /// <summary>
        /// Returns a singleton object of Job Request repository.
        /// </summary>
        /// <returns>The object of Job Request repository.</returns>
        public static IBasicRepository<JobRequest> GetJobRequestRepository()
        {
            if (jobRequestRepository == null)
            {
                CreateJobRequestRepository();
            }
            return jobRequestRepository;
        }

        private static void CreateJobRequestRepository()
        {
            var locker = new Object();
            jobRequestRepository = new JobRequestRepository(locker);
        }
    }
}
