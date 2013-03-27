using ClientPortal.Data.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name="Type")]
        public GoalTypeEnum TypeId { get; set; }

        [Display(Name="Offer")]
        public int? OfferId { get; set; }
        public string OfferName { get; set; }

        [Display(Name="Metric")]
        public MetricEnum MetricId { get; set; }

        [Required]
        public decimal Target { get; set; }

        public string TargetFormatted
        {
            get {
                if (TypeId == GoalTypeEnum.Absolute)
                {
                    if (MetricId == MetricEnum.Spend)
                        return String.Format(new CultureInfo(Culture), "{0:c}", Target);
                    else
                        return String.Format("{0:n0}", Target);
                }
                else // Percent
                {
                    return String.Format("{0:n1}", Target) + "%";
                }
            }
        }

        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }

        public GoalVM(Goal goal, string offerName, string currency)
        {
            this.Id = goal.Id;
            this.Name = goal.Name;
            this.TypeId = goal.TypeId;
            this.OfferId = goal.OfferId;
            this.OfferName = offerName;
            this.MetricId = goal.MetricId;
            this.Target = goal.Target;
            this.Currency = currency;
        }

        public GoalVM()
        { // defaults
            this.Id = -1;
            this.TypeId = GoalTypeEnum.Absolute;
            this.MetricId = MetricEnum.Leads;
        }
    }
}