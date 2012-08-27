using System.Data.EntityClient;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public partial class MediaBuyerWorkflowEntities
    {
        static public MediaBuyerWorkflowEntities Create()
        {
            string metaData = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", "Screens.MediaBuyerWorkflow.MediaBuyerWorkflowModel");
            return new MediaBuyerWorkflowEntities(new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = EomAppCommon.EomAppSettings.ConnStr + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = metaData
            }.ConnectionString);
        }
    }
}
