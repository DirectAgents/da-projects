using ClientPortal.Data.Contexts;
using System.Collections.Generic;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CampaignDropWizardVM
    {
        public enum WizardStep { ChooseCampaign, Details, Creatives, FinalReview, FinalReviewEdit };

        public WizardStep Step { get; set; }
        public Offer Offer { get; set; }

        public CampaignDrop CampaignDrop { get; set; }
        public IEnumerable<Creative> Creatives { get; set; }
        public Creative Creative { get; set; }
    }
}