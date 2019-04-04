using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using MVCGrid.Models;
using MVCGrid.Web;
using Ninject;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.Admin.Grids
{
    public static class JobHistoryGrid 
    {
        public static void AddGridConfiguration()
        {
            MVCGridDefinitionTable.Add("UsageExample", new MVCGridBuilder<JobRequestExecution>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("StartTime")
                        .WithHeaderText("StartTime")
                        .WithValueExpression(i => i.StartTime.ToString()); 
                    cols.Add().WithColumnName("EndTime")
                        .WithHeaderText("EndTime")
                        .WithValueExpression((i, c) => i.EndTime.ToString());
                    cols.Add().WithColumnName("ComamndName")
                        .WithHeaderText("CommandName")
                        .WithValueExpression((i, c) => i.JobRequest.CommandName);
                    cols.Add().WithColumnName("Arguments")
                        .WithHeaderText("Arguments")
                        .WithValueExpression((i, c) => i.JobRequest.CommandExecutionArguments);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((i, c) => i.JobRequest.Status.ToString());
                    cols.Add().WithColumnName("CurrentState")
                        .WithHeaderText("CurrentState")
                        .WithValueExpression((i, c) => i.CurrentState).WithCellCssClassExpression(i => "json-cell");
                    cols.Add().WithColumnName("Errors")
                       .WithHeaderText("Erorrs")
                       .WithValueExpression((i, c) => i.Errors).WithCellCssClassExpression(i=>"json-cell");
                    cols.Add().WithColumnName("Warnings")
                       .WithHeaderText("Warnings")
                       .WithValueExpression((i, c) => i.Warnings).WithCellCssClassExpression(i => "json-cell");
                })
                .WithClientSideLoadingCompleteFunctionName("dataLoadedCallback")
                .WithRetrieveDataMethod((context) =>
                {
                    var jobExecutionService = DependencyResolver.Current.GetService<IJobExecutionItemService>();
                    var items = jobExecutionService.GetJobExecutionHistoryItems();
                    return new QueryResult<JobRequestExecution>()
                    {
                        Items = items,
                        TotalRecords = 10 // if paging is enabled, return the total number of records of all pages
                    };
                })
            );
        }
    }
}