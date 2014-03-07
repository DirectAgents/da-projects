using System.Diagnostics;
namespace DirectAgents.Domain.Abstract
{
    public delegate void LogEventHandler(object sender, TraceEventType severity, string messageFormat, params object[] formatArgs);

    public interface IAdmin
    {
        event LogEventHandler LogHandler;

        void Test();

        void CreateDatabaseIfNotExists();
        void ReCreateDatabase();
        void LoadCampaigns();
        void LoadSummaries();
    }
}
