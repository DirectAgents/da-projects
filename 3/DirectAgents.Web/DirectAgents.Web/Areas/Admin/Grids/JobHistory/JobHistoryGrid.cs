using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Web.Areas.Admin.Grids.JobHistory;
using MVCGrid.Models;
using MVCGrid.Web;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.Admin.Grids
{
    public static class JobHistoryGrid
    {
        private const int JobHistoryPageSize = 20;

        public static void AddGridConfiguration()
        {
            MVCGridDefinitionTable.Add("JobHistory", new MVCGridBuilder<JobRequestExecution>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("ParentJobId")
                        .WithHeaderText("PJId")
                        .WithValueExpression(i => i.JobRequest.ParentJobRequestId.ToString())
                        .WithFiltering(true);
                    cols.Add().WithColumnName("JobId")
                        .WithHeaderText("JId")
                        .WithValueExpression(i => i.JobRequest.Id.ToString());
                    cols.Add().WithColumnName("CommandName")
                        .WithHeaderText("Command")
                        .WithValueExpression((i, c) => i.JobRequest.CommandName)
                        .WithFiltering(true);
                    cols.Add().WithColumnName("Arguments")
                        .WithHeaderText("Args")
                        .WithValueExpression((i, c) => i.JobRequest.CommandExecutionArguments);
                    cols.Add().WithColumnName("StartTime")
                       .WithHeaderText("Start")
                       .WithValueExpression(i => i.StartTime.ToString())
                       .WithFiltering(true);
                    cols.Add().WithColumnName("EndTime")
                        .WithHeaderText("End")
                        .WithValueExpression((i, c) => i.EndTime.ToString());
                    cols.Add().WithColumnName("ElapsedTime")
                        .WithHeaderText("ElapsedTime")
                        .WithValueExpression((i, c) => (i.StartTime.HasValue && i.EndTime.HasValue) ? ((i.EndTime - i.StartTime).ToString()) : null);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((i, c) => i.Status.ToString())
                        .WithFiltering(true);
                    cols.Add().WithColumnName("CurrentState")
                        .WithHeaderText("CurrentState")
                        .WithValueExpression((i, c) => i.CurrentState).WithCellCssClassExpression(i => "json-cell state");
                    cols.Add().WithColumnName("Errors")
                       .WithHeaderText("Erorrs")
                       .WithValueExpression((i, c) => i.Errors).WithCellCssClassExpression(i => "json-cell errors");
                    cols.Add().WithColumnName("Warnings")
                       .WithHeaderText("Warnings")
                       .WithValueExpression((i, c) => i.Warnings).WithCellCssClassExpression(i => "json-cell warnings");
                })
                .WithClientSideLoadingCompleteFunctionName("dataLoadedCallback")
                .WithPaging(true, JobHistoryPageSize)
                .WithFiltering(true)
                .WithRetrieveDataMethod((context) =>
                {
                    QueryOptions options = context.QueryOptions;
                    var dataProvider = DependencyResolver.Current.GetService<IJobHistoryDataProvider>();
                    return dataProvider.GetQueryResult(options);
                })
            );
        }
    }
}