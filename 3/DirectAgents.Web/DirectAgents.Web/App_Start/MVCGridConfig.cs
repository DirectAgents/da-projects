[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DirectAgents.Web.MVCGridConfig), "RegisterGrids")]

namespace DirectAgents.Web
{
    using DirectAgents.Web.Areas.Admin.Grids;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            JobHistoryGrid.AddGridConfiguration();
        }
    }
}