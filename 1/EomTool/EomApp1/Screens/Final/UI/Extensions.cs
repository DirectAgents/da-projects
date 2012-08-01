using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Final.UI
{
    public static class Extensions
    {
        public static List<int> ItemIds(this Data.CampaignsDataTable campaigns, int pid)
        {
            return campaigns.Where(c => c.PID == pid).Single().ItemIds
                            .Split(',').Select(c => int.Parse(c))
                            .ToList();
        }
    }
}
