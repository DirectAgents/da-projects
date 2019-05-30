using System.Web.Mvc;
using DirectAgents.Web.Areas.Admin.Grids.DataProviders;
using MVCGrid.Models;
using MVCGrid.Web;

namespace DirectAgents.Web.Areas.Admin.Grids.JobRequest
{
    /// <summary>
    /// Job Request Grid Configuration.
    /// </summary>
    public static class JobRequestsGrid
    {
        private const int JobRequestsPageSize = 20;

        /// <summary>
        /// Adds the grid configuration.
        /// </summary>
        public static void AddGridConfiguration()
        {
            MVCGridDefinitionTable.Add("JobRequests", new MVCGridBuilder<Domain.Entities.Administration.JobExecution.JobRequest>()
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
                        .WithValueExpression(i => i.ParentJobRequestId.ToString());
                    cols.Add().WithColumnName("JobId")
                        .WithHeaderText("JId")
                        .WithValueExpression(i => i.Id.ToString());
                    cols.Add().WithColumnName("CommandName")
                        .WithHeaderText("Command")
                        .WithValueExpression((i, c) => i.CommandName);
                    cols.Add().WithColumnName("Arguments")
                        .WithHeaderText("Args")
                        .WithValueExpression((i, c) => i.CommandExecutionArguments);
                    cols.Add().WithColumnName("ScheduledTime")
                        .WithHeaderText("Schedule")
                        .WithValueExpression(i => i.ScheduledTime.ToString());
                    cols.Add().WithColumnName("AttemptNumber")
                        .WithHeaderText("Attempt")
                        .WithValueExpression((i, c) => i.AttemptNumber.ToString());
                    cols.Add().WithColumnName("Status")
                        .WithHeaderText("Status")
                        .WithValueExpression((i, c) => i.Status.ToString())
                        .WithFiltering(true);
                })
                .WithClientSideLoadingCompleteFunctionName("dataLoadedCallback")
                .WithPaging(true, JobRequestsPageSize)
                .WithFiltering(true)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var dataProvider = DependencyResolver.Current.GetService<IGridDataProvider<Domain.Entities.Administration.JobExecution.JobRequest>>();
                    return dataProvider.GetQueryResult(options);
                }));
        }
    }
}