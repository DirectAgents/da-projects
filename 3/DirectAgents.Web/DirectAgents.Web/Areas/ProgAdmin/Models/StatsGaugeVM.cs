using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class StatsGaugeVM
    {
        public string PlatformCode { get; set; }
        public int? CampaignId { get; set; }
        public bool Extended { get; set; }

        public IEnumerable<TDStatsGauge> StatsGauges { get; set; }

        // The top-level StatsGauges are platform summaries; so if set, the child StatsGauges have the individual ExtAccounts.
        public IEnumerable<ExtAccount> ExtAccounts(bool syncableOnly = false)
        {
            var extAccts = StatsGauges.Where(x => x.Children != null).SelectMany(x => x.Children).Where(x => x.ExtAccount != null).Select(x => x.ExtAccount);
            if (syncableOnly)
            {
                var codesSyncable = Platform.Codes_Syncable();
                extAccts = extAccts.Where(x => codesSyncable.Contains(x.Platform.Code));
            }
            return extAccts;
        }

        public List<SelectListItem> ExtAccountSelectListItems(bool syncableOnly = false, bool includePlatforms = false)
        {
            var slItems = new List<SelectListItem>();
            var extAccounts = ExtAccounts(syncableOnly: syncableOnly);
            var extAcctGroups = extAccounts.GroupBy(x => x.Platform);
            foreach (var platGroup in extAcctGroups)
            {
                foreach (var extAcct in platGroup)
                {
                    slItems.Add(new SelectListItem { Text = extAcct.DisplayName1, Value = extAcct.Id.ToString() });
                }
                if (includePlatforms && (platGroup.Count() > 1) && (!syncableOnly || platGroup.Key.Code == Platform.Code_FB))
                    slItems.Add(new SelectListItem { Text = "(" + platGroup.Key.Name + ") <<all for this campaign>>", Value = (0 - platGroup.Key.Id).ToString() });
                    // Set the value to the negative of the platformId
            }
            return slItems;
        }
    }
}