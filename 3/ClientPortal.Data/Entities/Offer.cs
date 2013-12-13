using ClientPortal.Data.Contexts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public partial class Offer
    {
        [NotMapped]
        public string DisplayName
        {
            get { return OfferName + " (" + OfferId + ")"; }
        }

        [NotMapped]
        public Advertiser Advertiser { get; set; }

        public Offer ThisWithAdvertiserInfo(int advertiserId, string advertiserName)
        {
            this.Advertiser = new Advertiser
            {
                AdvertiserId = advertiserId,
                AdvertiserName = advertiserName
            };
            return this;
        }

        public IOrderedEnumerable<Creative> CreativesByDate()
        {
            return this.Creatives.OrderByDescending(c => c.DateCreated);
        }

        private DropReport dropReport;
        [NotMapped]
        public DropReport DropReport
        {
            get
            {
                if (dropReport == null)
                    dropReport = new DropReport(Campaigns.SelectMany(c => c.CampaignDrops));
                return dropReport;
            }
        }
    }

    public class DropReport
    {
        public DropReport(IEnumerable<CampaignDrop> campaignDrops)
        {
            CampaignDrops = campaignDrops;

            TotalVolume = campaignDrops.Sum(cd => cd.Volume ?? 0);
            TotalOpens = campaignDrops.Sum(cd => cd.Opens ?? 0);
            TotalClicks = campaignDrops.Sum(cd => cd.CreativeStatTotals.Clicks ?? 0);
            TotalLeads = campaignDrops.Sum(cd => cd.CreativeStatTotals.Leads ?? 0);
            TotalCost = campaignDrops.Sum(cd => cd.Cost ?? 0);
        }

        public IEnumerable<CampaignDrop> CampaignDrops { get; set; }
        public int TotalVolume { get; set; }
        public int TotalOpens { get; set; }
        public int TotalClicks { get; set; }
        public int TotalLeads { get; set; }
        public decimal TotalCost { get; set; }

        public decimal OpenRate
        {
            get { return (TotalVolume == 0) ? 0 : (decimal)TotalOpens / TotalVolume; }
        }

        public decimal ClickThroughRate
        {
            get { return (TotalOpens == 0) ? 0 : (decimal)TotalClicks / TotalOpens; }
        }

        public decimal ConversionRate
        {
            get { return (TotalClicks == 0) ? 0 : (decimal)TotalLeads / TotalClicks; }
        }

        public decimal CostPerLead
        {
            get { return (TotalLeads == 0) ? 0 : (TotalCost / TotalLeads); }
        }
    }
}
