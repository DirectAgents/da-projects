using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations
{
    public class TestJobRequestRepository : IBaseRepository<JobRequest>
    {
        public ConcurrentBag<JobRequest> ScheduledRequests = new ConcurrentBag<JobRequest>();

        public string EntityName { get; }

        public JobRequest GetItem(int id)
        {
            return new JobRequest();
        }

        public JobRequest GetFirstItem(Func<JobRequest, bool> predicate)
        {
            return new JobRequest();
        }

        public List<JobRequest> GetItems(Func<JobRequest, bool> predicate)
        {
            return new List<JobRequest>();
        }

        public List<JobRequest> GetItemsWithIncludes(Func<JobRequest, bool> predicate, string includeProperty)
        {
            throw new NotImplementedException();
        }

        public void AddItem(JobRequest item)
        {
        }

        public void AddItems(IEnumerable<JobRequest> items)
        {
            foreach (var jobRequest in items)
            {
                ScheduledRequests.Add(jobRequest);
            }
        }

        public void UpdateItem(JobRequest itemToUpdate)
        {
        }

        public void UpdateItems(IEnumerable<JobRequest> itemsToUpdate)
        {
        }

        public bool MergeItems(IEnumerable<JobRequest> itemsToMerge)
        {
            return true;
        }

        public bool MergeItems(IEnumerable<JobRequest> itemsToMerge, Action entityBulkOptionsAction)
        {
            return true;
        }

        public bool MergeItems(IEnumerable<JobRequest> itemsToMerge, Action<EntityBulkOperation<JobRequest>> entityBulkOptionsAction)
        {
            return true;
        }
    }
}
