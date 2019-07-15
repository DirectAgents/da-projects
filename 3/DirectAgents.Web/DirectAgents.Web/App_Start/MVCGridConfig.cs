using DirectAgents.Web.Areas.Admin.Grids.JobHistory;
using DirectAgents.Web.Areas.Admin.Grids.JobRequest;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DirectAgents.Web.MVCGridConfig), "RegisterGrids")]

namespace DirectAgents.Web
{
    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            JobHistoryGrid.AddGridConfiguration();
            JobRequestsGrid.AddGridConfiguration();
        }
    }
}