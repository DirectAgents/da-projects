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

    }
}
