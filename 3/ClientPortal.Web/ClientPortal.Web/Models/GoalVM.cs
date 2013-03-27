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
                    return FormatSomeTarget(Target);
                else // Percent
                    return String.Format("{0:n1}", Target) + "%";
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

        // e.g. "1,000", "$1,000", "10.5% (1,105)", "10.5% ($1,105)"
        public string TargetFormattedBasedOn(DateRangeSummary rangeSummary)
        {
            if (TypeId == GoalTypeEnum.Absolute)
                return TargetFormatted;
            else // Percent
                return TargetFormatted + " (" + FormatSomeTarget(TargetBasedOn(rangeSummary)) + ")";
        }

        public decimal TargetBasedOn(DateRangeSummary rangeSummary)
        {
            if (TypeId == GoalTypeEnum.Absolute)
                return Target;
            else
            { // for Percent goal...
                var baseVal = ValueFor(rangeSummary);
                var multiplier = 1 + (Target / 100);
                return baseVal * multiplier;
            }
        }

        public decimal ValueFor(DateRangeSummary rangeSummary)
        {
            switch (MetricId)
            {
                case MetricEnum.Clicks:
                    return rangeSummary.Clicks;
                case MetricEnum.Leads:
                    return rangeSummary.Conversions;
                case MetricEnum.Spend:
                    return rangeSummary.Revenue;
                default:
                    return 0;
            }
        }

        // --- helper methods ---

        private string FormatSomeTarget(decimal someTarget) // ...based on this goal's metric
        {
            if (MetricId == MetricEnum.Spend)
                return String.Format(new CultureInfo(Culture), "{0:c}", someTarget);
            else
                return String.Format("{0:n0}", someTarget);
        }
    }
}