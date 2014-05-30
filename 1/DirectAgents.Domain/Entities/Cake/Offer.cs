using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class Offer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfferId { get; set; }

        public Nullable<int> AdvertiserId { get; set; }
        public virtual Advertiser Advertiser { get; set; }

        public string OfferName { get; set; }
        public string DefaultPriceFormatName { get; set; }
        public string CurrencyAbbr { get; set; }
        public DateTime DateCreated { get; set; }

        // --- OfferInfo table ---

        public Nullable<decimal> Budget { get; set; }
        public bool BudgetIsMonthly { get; set; }
        public DateTime? BudgetStart { get; set; }

        // --- misc ---

        [NotMapped]
        public string BudgetIsMonthlyString
        {
            get
            {
                return (!Budget.HasValue ? "" : (BudgetIsMonthly ? "yes" : "no"));
            }
        }
    }
}
