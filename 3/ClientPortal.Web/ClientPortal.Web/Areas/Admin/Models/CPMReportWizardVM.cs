using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CPMReportWizardVM
    {
        public enum WizardStep { OfferLogo, SynchCampaigns, SynchCreatives, Test };

        public Offer Offer { get; set; }
        public WizardStep Step { get; set; }
    }
}