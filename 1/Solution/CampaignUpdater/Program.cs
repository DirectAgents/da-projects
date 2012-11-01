using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;

namespace CampaignUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("start CampaignUpdater");
            IAdmin admin = new AdminImpl();
            admin.LogHandler += admin_LogHandler;

            admin.LoadCampaigns();
            admin.LoadSummaries();

            Logger.Log("end CampaignUpdater");
        }

        static void admin_LogHandler(object sender, string messageFormat, params object[] formatArgs)
        {
            Logger.Log(messageFormat, formatArgs);
        }
    }
}
