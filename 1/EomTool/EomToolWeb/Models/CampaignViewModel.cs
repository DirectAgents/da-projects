using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DirectAgents.Domain.Entities.Wiki;

namespace EomToolWeb.Models
{
    public class CampaignViewModel
    {
        private Campaign campaign;

        public CampaignViewModel(Campaign campaign)
        {
            this.campaign = campaign;
        }
        public CampaignViewModel(Campaign campaign, decimal? availableBudget)
        {
            this.campaign = campaign;
            this.AvailableBudget = availableBudget;
        }

        public string AdManagers
        {
            get
            {
                string result = string.Join(", ", campaign.AdManagers.Select(am => am.Name));
                return result;
            }
        }

        public string AccountManagers
        {
            get
            {
                string result = string.Join(", ", campaign.AccountManagers.Select(am => am.Name));
                return result;
            }
        }

        public string CountryCodes
        {
            get
            {
                var countryCodes = campaign.Countries.Select(c => c.CountryCode).ToArray();
                return string.Join(" ", countryCodes);
            }
        }

        public string TrafficTypes
        {
            get
            {
                var trafficTypes = campaign.TrafficTypes.Select(t => t.Name).ToArray();
                return string.Join(", ", trafficTypes);
            }
        }

        public string Name { get { return campaign.Name; } }

        public string Description { get { return campaign.Description; } }
        public string DescriptionTrimmed {
            get
            {
                var desc = campaign.Description;
                if (campaign.Description.Length > 220)
                    desc = campaign.Description.Substring(0, 220) + ".....";
                return desc;
            }
        }

        public int Pid { get { return campaign.Pid; } }

        public string ImageUrl
        {
            get
            {
                return string.IsNullOrWhiteSpace(campaign.ImageUrl)
                    ? VirtualPathUtility.ToAbsolute("~/Images/noimage_sm.gif")
                    : campaign.ImageUrl; ;
            }
        }

        public string PayableAction { get { return campaign.PayableAction ?? string.Empty; } }

        public string Link { get { return campaign.Link; } }

        public string CostCurrency { get { return campaign.CostCurrency; } }
        public decimal Cost { get { return campaign.Cost; } }

        public string RevenueCurrency { get { return campaign.RevenueCurrency; } }
        public decimal Revenue { get { return campaign.Revenue; } }

        public bool RevenueIsPercentage { get { return campaign.RevenueIsPercentage; } }

        public decimal? AvailableBudget { get; set; }

        public string ImportantDetails { get { return campaign.ImportantDetails ?? string.Empty; } }
        public string ImportantDetailsHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(campaign.ImportantDetails))
                    return string.Empty;
                else
                    return MakeHtmlSafe(campaign.ImportantDetails).Replace(System.Environment.NewLine, "<br />").Replace("\n", "<br />");
            }
        }

        public string Restrictions { get { return campaign.Restrictions ?? string.Empty; } }
        public string RestrictionsHtml {
            get {
                if (string.IsNullOrWhiteSpace(campaign.Restrictions))
                    return string.Empty;
                else
                    return campaign.Restrictions.Replace(System.Environment.NewLine, "<br />").Replace("\n", "<br />");
            }
        }

        public string Budget { get { return campaign.Budget ?? string.Empty; } }

        public string PassedInfo { get { return campaign.PassedInfo ?? string.Empty; } }

        public string CampaignCap { get { return campaign.CampaignCap ?? string.Empty; } }

        public string ScrubPolicy { get { return campaign.ScrubPolicy ?? string.Empty; } }

        public string EomNotes { get { return campaign.EomNotes ?? string.Empty; } }

        public string Vertical { get { return campaign.Vertical.Name; } }

        public string Status { get { return campaign.Status.Name; } }
        public bool Hidden { get { return campaign.Hidden; } }

        public bool CPM { get { return campaign.Name.ToLower().Contains("cpm"); } }

        public string DefaultPriceFormat { get { return campaign.DefaultPriceFormat; } }

//        public string MobileAllowed { get { return campaign.MobileAllowed ?? string.Empty; } }
        public string MobileLP { get { return campaign.MobileLP ?? string.Empty; } }

        //TODO: Put in utility class
        private string MakeHtmlSafe(string text)
        {
            if (text == null) return null;

            StringBuilder sb = new StringBuilder(
                HttpUtility.HtmlEncode(text));
            // Selectively allow <b> and <i>
            sb.Replace("&lt;b&gt;", "<b>");
            sb.Replace("&lt;/b&gt;", "</b>");
            sb.Replace("&lt;i&gt;", "<i>");
            sb.Replace("&lt;/i&gt;", "</i>");
            return Regex.Replace(sb.ToString(), "&lt;(br */?)&gt;", "<$1>"); // allow <br>'s
        }

    }
}