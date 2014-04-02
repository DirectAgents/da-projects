using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CPMReportWizardVM
    {
        public enum WizardStep { OfferLogo, Synch, ReviewDrops, CreateReport, AddDrops, EditReport };

        public WizardStep Step { get; set; }

        public CPMReport Report { get; set; }
        public Offer Offer { get; set; }
    }
}