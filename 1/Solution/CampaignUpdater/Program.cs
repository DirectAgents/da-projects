using DirectAgents.Common.Logging;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using System.Diagnostics;

namespace CampaignUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.InitializeLogging();

            Logger.Info("start CampaignUpdater");
            IAdmin admin = new AdminImpl();
            admin.LogHandler += admin_LogHandler;

            admin.LoadCampaigns();
            admin.LoadSummaries();

            Logger.Info("end CampaignUpdater");
        }

        static void admin_LogHandler(object sender, TraceEventType severity, string messageFormat, params object[] formatArgs)
        {
            if (severity == TraceEventType.Information)
                Logger.Info(messageFormat, formatArgs);
            else
                Logger.Warn(messageFormat, formatArgs);
        }
    }
}
