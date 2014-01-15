using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public class CPMReport_Validation
    {
        [Required]
        public string Name { get; set; }
    }

    [MetadataType(typeof(CPMReport_Validation))]
    public partial class CPMReport
    {
        [NotMapped]
        public IEnumerable<CampaignDrop> CampaignDropsOrdered
        {
            get { return CampaignDrops.OrderBy(cd => cd.Date); }
            //TODO: what to order on next? (if multiple drops on the same date)
        }

        [NotMapped]
        public decimal OpenRate
        {
            get { return (TotalVolume == 0) ? 0 : (decimal)TotalOpens / TotalVolume; }
        }
        [NotMapped]
        public decimal ClickThroughRate
        {
            get { return (TotalOpens == 0) ? 0 : (decimal)TotalClicks / TotalOpens; }
        }
        [NotMapped]
        public decimal ConversionRate
        {
            get { return (TotalClicks == 0) ? 0 : (decimal)TotalLeads / TotalClicks; }
        }
        [NotMapped]
        public decimal CostPerLead
        {
            get { return (TotalLeads == 0) ? 0 : (TotalCost / TotalLeads); }
        }

        //NOTE: This is designed for one web request (not a persistent object where the drops are modified)
        private int? _totalVolume;
        private int? _totalOpens;
        private int? _totalClicks;
        private int? _totalLeads;
        private decimal? _totalCost;

        [NotMapped]
        public int TotalVolume
        {
            get
            {
                if (_totalVolume == null) _totalVolume = CampaignDrops.Sum(cd => cd.Volume ?? 0);
                return _totalVolume.Value;
            }
        }

        [NotMapped]
        public int TotalOpens
        {
            get
            {
                if (_totalOpens == null) _totalOpens = CampaignDrops.Sum(cd => cd.Opens ?? 0);
                return _totalOpens.Value;
            }
        }

        [NotMapped]
        public int TotalClicks
        {
            get
            {
                if (_totalClicks == null) _totalClicks = CampaignDrops.Sum(cd => cd.CreativeStatTotals.Clicks ?? 0);
                return _totalClicks.Value;
            }
        }

        [NotMapped]
        public int TotalLeads
        {
            get
            {
                if (_totalLeads == null) _totalLeads = CampaignDrops.Sum(cd => cd.CreativeStatTotals.Leads ?? 0);
                return _totalLeads.Value;
            }
        }

        [NotMapped]
        public decimal TotalCost
        {
            get
            {
                if (_totalCost == null) _totalCost = CampaignDrops.Sum(cd => cd.Cost ?? 0);
                return _totalCost.Value;
            }
        }

    }
}
