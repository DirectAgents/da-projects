using DirectAgents.Domain.Entities.Administration.JobExecution;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution
{
    public interface IJobExecutionItemRepository
    {
        void UpdateItem(JobRequestExecution jobRequestExecution);

        JobRequestExecution AddItem(JobRequestExecution JobRequestExecution);

        List<JobRequestExecution> GetAll(Expression<Func<JobRequestExecution, bool>> whereCondition);
    }
}
