using ClientPortal.Data.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class GoalVM : IValidatableObject
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

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public bool IsMonthly
        {
            get { return (!StartDate.HasValue && !EndDate.HasValue); }
        }

        // ? possibly have this return true iff there are validation errors on the dates ?
        public bool ShowCustomDateRange
        {
            get { return (StartDate.HasValue || EndDate.HasValue); }
        }

        public string CreateGoalChartCall { get; set; }

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
            this.StartDate = goal.StartDate;
            this.EndDate = goal.EndDate;
        }

        public GoalVM()
        { // defaults
            this.Id = -1;
            this.TypeId = GoalTypeEnum.Absolute;
            this.MetricId = MetricEnum.Leads;
        }

        // todo: share this code with Goal entity (currently duplicated)
        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!StartDate.HasValue && EndDate.HasValue)
                yield return new ValidationResult("Must provide a Start Date.", new[] { "StartDate" });
            if (StartDate.HasValue && !EndDate.HasValue)
                yield return new ValidationResult("Must provide an End Date.", new[] { "EndDate" });
            if (StartDate.HasValue && EndDate.HasValue && StartDate >= EndDate)
                yield return new ValidationResult("Start Date must be before End Date.", new[] { "StartDate" });
        }

        // e.g. "Reach 1,000 Leads", "Reach $1,000 Spend", "Increase Leads 10.5% (to 1,105)", "Increase Spend 10.5% (to $1,105)"
        public string TargetFormattedBasedOn(DateRangeSummary rangeSummary)
        {
            if (TypeId == GoalTypeEnum.Absolute)
                return "Reach " + TargetFormatted + " " + this.MetricId;
            else // Percent
                return "Increase " + this.MetricId + " " + TargetFormatted + " (to " + FormatSomeTarget(TargetBasedOn(rangeSummary)) + ")";
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