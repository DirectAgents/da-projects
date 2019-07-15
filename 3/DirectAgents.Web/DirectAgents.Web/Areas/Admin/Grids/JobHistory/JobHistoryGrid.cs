using System.Web.Mvc;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Web.Areas.Admin.Grids.DataProviders;
using MVCGrid.Models;
using MVCGrid.Web;

namespace DirectAgents.Web.Areas.Admin.Grids.JobHistory
{
    /// <summary>
    /// Job History Grid Configuration.
    /// </summary>
    public static class JobHistoryGrid
    {
        private const int JobHistoryPageSize = 20;

        /// <summary>
        /// Adds the grid configuration.
        /// </summary>
        public static void AddGridConfiguration()
        {
            MVCGridDefinitionTable.Add("JobHistory", new MVCGridBuilder<JobRequestExecution>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
                .AddColumns(cols =>
                {
                    cols.Add("Abort")
                         .WithHtmlEncoding(false)
                         .WithSorting(false)
                         .WithFiltering(false)
                         .WithHeaderText("")
                         .WithValueExpression((p, c) => $"<input type='checkbox' class='select' value='{p.Id}'>")
                         .WithPlainTextValueExpression((p, c) => "");
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
                        .WithHeaderText("Elapsed")
                        .WithValueExpression((i, c) => (i.StartTime.HasValue && i.EndTime.HasValue) ? (i.EndTime - i.StartTime).Value.ToString(@"hh\:mm\:ss") : null);
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((i, c) => i.Status.ToString())
                        .WithFiltering(true);
                    cols.Add().WithColumnName("CurrentState")
                        .WithHeaderText("CurrentState")
                        .WithValueExpression((i, c) => i.CurrentState).WithCellCssClassExpression(i => "json-cell state");
                    cols.Add().WithColumnName("Errors")
                       .WithHeaderText("Errors")
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
                    var dataProvider = DependencyResolver.Current.GetService<IGridDataProvider<JobRequestExecution>>();
                    return dataProvider.GetQueryResult(options);
                }));
        }
    }
}