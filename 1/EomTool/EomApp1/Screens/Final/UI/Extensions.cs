using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Final.UI
{
    public static class Extensions
    {
        public static List<int> ItemIds(this Data.CampaignsDataTable campaigns, int pid, string currency)
        {
            var camp = campaigns.Where(c => c.PID == pid && c.Currency == currency);
            List<int> ids = new List<int>();
            foreach (var campaign in camp)
            {
                var campIds = campaign.ItemIds.Split(',').Select(c => int.Parse(c));
                ids.AddRange(campIds);
            }
            return ids;
        }
    }
}
