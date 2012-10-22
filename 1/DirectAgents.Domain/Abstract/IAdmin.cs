namespace DirectAgents.Domain.Abstract
{
    public delegate void LogEventHandler(object sender, string messageFormat, params object[] formatArgs);

    public interface IAdmin
    {
        event LogEventHandler LogHandler;

        void CreateDatabaseIfNotExists();
        void ReCreateDatabase();
        void LoadCampaigns();
        void LoadSummaries();
    }
}
