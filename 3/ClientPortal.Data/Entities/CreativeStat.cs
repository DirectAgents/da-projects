using ClientPortal.Data.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Contexts
{
    public partial class CreativeStat
    {
        [NotMapped]
        public decimal? ClickThroughRate
        {
            get
            {
                if (Clicks == null || CampaignDrop.Opens == null)
                    return null;
                else
                    return (decimal)Clicks.Value / CampaignDrop.Opens.Value;
            }
        }

        public void SetPropertiesFrom(CreativeStat inStat)
        {
            this.CreativeId = inStat.CreativeId;
            this.Clicks = inStat.Clicks;
            this.Leads = inStat.Leads;
        }

        public bool AnyChanges(CreativeStat inStat)
        {
            bool anyChanges = false;

            if (this.CreativeId != inStat.CreativeId ||
                this.Clicks != inStat.Clicks ||
                this.Leads != inStat.Leads)
                anyChanges = true;

            return anyChanges;
        }
    }
}
