using DirectAgents.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public virtual List<OfferBudget> OfferBudgets { get; set; }

        // --- OfferInfo table ---
        public bool BudgetIsMonthly { get; set; }

        public OfferBudget OfferBudget
        {
            get
            {
                if (OfferBudgets == null)
                    OfferBudgets = new List<OfferBudget>();

                if (OfferBudgets.Count == 0)
                    OfferBudgets.Add(new OfferBudget());

                return OfferBudgets[0];
            }
        }

        [NotMapped]
        public bool HasBudget
        {
            get { return (OfferBudgets == null || OfferBudgets.Count > 0); }
        }

        [NotMapped]
        public decimal? Budget
        {
            get { return HasBudget ? (decimal?)OfferBudget.Budget : null; }
            set { if (value.HasValue) OfferBudget.Budget = value.Value; }
        }
        [NotMapped]
        public DateTime? BudgetStart
        {
            get { return HasBudget ? (DateTime?)OfferBudget.Start : null; }
            set { if (value.HasValue) OfferBudget.Start = value.Value; }
        }
        [NotMapped]
        public DateTime? BudgetEnd
        {
            get { return HasBudget ? (DateTime?)OfferBudget.End : null; }
            set { if (value.HasValue) OfferBudget.End = value.Value; }
        }

        // --- misc ---

        [NotMapped]
        public string BudgetIsMonthlyString
        {
            get
            {
                return (BudgetIsMonthly ? "yes" : "no");
            }
        }

        [NotMapped]
        public decimal? BudgetUsed { get; set; }
        [NotMapped]
        public decimal? BudgetAvailable
        {
            get
            {
                if (!Budget.HasValue || !BudgetUsed.HasValue)
                    return null;
                if (BudgetUsed.Value >= Budget.Value)
                    return 0;
                else
                    return Budget.Value - BudgetUsed.Value;
            }
        }
        [NotMapped]
        public decimal? BudgetUsedPercent
        {
            get
            {
                if (!Budget.HasValue || Budget.Value <= 0 || !BudgetUsed.HasValue)
                    return null;
                return BudgetUsed.Value / Budget.Value;
            }
        }

        [NotMapped]
        public DateTime? EarliestStatDate { get; set; }
        [NotMapped]
        public DateTime? LatestStatDate { get; set; }
    }
}
