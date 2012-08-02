using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Final.UI
{
    public static class Extensions
    {
        public static List<int> ItemIds(this Data.CampaignsDataTable campaigns, int pid, string currency)
        {
            return campaigns.Single(c => c.PID == pid && c.Currency == currency).ItemIds
                            .Split(',').Select(c => int.Parse(c))
                            .ToList();
        }
    }
}
