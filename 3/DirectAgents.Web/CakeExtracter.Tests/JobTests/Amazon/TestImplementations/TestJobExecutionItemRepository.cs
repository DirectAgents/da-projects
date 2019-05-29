using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution;

namespace CakeExtracter.Tests.JobTests.Amazon.TestImplementations
{
    public class TestJobExecutionItemRepository : IJobExecutionItemRepository
    {
        public void UpdateItem(JobRequestExecution jobRequestExecution)
        {
        }

        public JobRequestExecution AddItem(JobRequestExecution JobRequestExecution)
        {
            return new JobRequestExecution();
        }

        public List<JobRequestExecution> GetAll(Expression<Func<JobRequestExecution, bool>> whereCondition)
        {
            return new List<JobRequestExecution>();
        }
    }
}
