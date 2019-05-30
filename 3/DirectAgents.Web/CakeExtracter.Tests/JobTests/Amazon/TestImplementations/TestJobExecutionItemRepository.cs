using System;
using System.Collections.Generic;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using Z.EntityFramework.Extensions;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations
{
    public class TestJobExecutionItemRepository : IBaseRepository<JobRequestExecution>
    {
        public string EntityName { get; }

        public void AddItem(JobRequestExecution item)
        {
        }

        public void AddItems(IEnumerable<JobRequestExecution> items)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(JobRequestExecution jobRequestExecution)
        {
        }

        public void UpdateItems(IEnumerable<JobRequestExecution> itemsToUpdate)
        {
            throw new NotImplementedException();
        }

        public bool MergeItems(IEnumerable<JobRequestExecution> itemsToMerge)
        {
            throw new NotImplementedException();
        }

        public bool MergeItems(IEnumerable<JobRequestExecution> itemsToMerge, Action<EntityBulkOperation<JobRequestExecution>> entityBulkOptionsAction)
        {
            throw new NotImplementedException();
        }

        public JobRequestExecution GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public JobRequestExecution GetFirstItem(Func<JobRequestExecution, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<JobRequestExecution> GetItems(Func<JobRequestExecution, bool> predicate)
        {
            return new List<JobRequestExecution>();
        }

        public List<JobRequestExecution> GetItemsWithIncludes(Func<JobRequestExecution, bool> predicate, string includeProperty)
        {
            throw new NotImplementedException();
        }
    }
}
