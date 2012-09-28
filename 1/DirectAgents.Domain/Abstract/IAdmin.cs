namespace DirectAgents.Domain.Abstract
{
    public interface IAdmin
    {
        void CreateDatabaseIfNotExists();
        void ReCreateDatabase();
        void LoadCampaigns();
        void LoadSummaries();
    }
}
