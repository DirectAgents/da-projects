﻿using ClientPortal.Data.DTOs;
using ClientPortal.Web.Controllers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class GoalVM : IValidatableObject
    {
        public int Id { get; set; }

        public string OfferGoalId
        {
            get { return (OfferId.HasValue ? OfferId.Value.ToString() : "") + "_" + Id; }
        }

        public int AdvertiserId { get; set; }

        [Display(Name="Offer")]
        public int? OfferId { get; set; }
        public string OfferName { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public GoalTypeEnum TypeId { get; set; }

        [Display(Name = "Metric")]
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

        //public string Currency
        //{
        //    set { Culture = OfferInfo.CurrencyToCulture(value); }
        //}
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public bool IsMonthly
        {
            get { return (String.IsNullOrEmpty(StartDate) && String.IsNullOrEmpty(EndDate)); }
        }

        // ? possibly have this return true iff there are validation errors on the dates ?
        public bool ShowCustomDateRange
        {
            get { return (!String.IsNullOrWhiteSpace(StartDate) || !String.IsNullOrWhiteSpace(EndDate)); }
        }

        public string CreateGoalChartCall { get; set; }

        public GoalVM(Data.Contexts.Goal goal, string offerName, string culture)
        {
            this.Id = goal.Id;
            this.AdvertiserId = goal.AdvertiserId;
            this.OfferId = goal.OfferId;
            this.OfferName = offerName;
            this.Name = goal.Name;
            this.TypeId = (GoalTypeEnum)goal.TypeId;
            this.MetricId = (MetricEnum)goal.MetricId;
            this.Target = goal.Target;
            this.Culture = culture;
            if (goal.StartDate.HasValue)
                this.StartDate = goal.StartDate.Value.ToString("d", CultureInfo);
            if (goal.EndDate.HasValue)
                this.EndDate = goal.EndDate.Value.ToString("d", CultureInfo);
        }
        public DateTime? StartDateParsed
        {
            get { return ControllerHelpers.ParseDate(StartDate, CultureInfo); }
        }
        public DateTime? EndDateParsed
        {
            get { return ControllerHelpers.ParseDate(EndDate, CultureInfo); }
        }

        public GoalVM()
        { // defaults
            this.Id = -1;
            this.TypeId = GoalTypeEnum.Absolute;
            this.MetricId = MetricEnum.Leads;
        }

        public void SetGoalEntityProperties(Goal goal)
        {
            goal.AdvertiserId = this.AdvertiserId;
            goal.OfferId = this.OfferId;
            goal.Name = this.Name;
            goal.TypeId = this.TypeId;
            goal.MetricId = this.MetricId;
            goal.Target = this.Target;

            goal.StartDate = StartDateParsed;
            goal.EndDate = EndDateParsed;
        }

        // todo: share this code with Goal entity (currently duplicated)
        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            DateTime? startDate, endDate;
            bool startDateParsed = ControllerHelpers.ParseDate(StartDate, CultureInfo, out startDate);
            bool endDateParsed = ControllerHelpers.ParseDate(EndDate, CultureInfo, out endDate);

            if (!startDateParsed)
                yield return new ValidationResult("Not a valid date.", new[] { "StartDate" });
            else if (startDate.HasValue && endDateParsed && !endDate.HasValue)
                yield return new ValidationResult("Must provide an End Date.", new[] { "EndDate" });

            if (!endDateParsed)
                yield return new ValidationResult("Not a valid date.", new[] { "EndDate" });
            else if (endDate.HasValue && startDateParsed && !startDate.HasValue)
                yield return new ValidationResult("Must provide a Start Date.", new[] { "StartDate" });

            if (startDateParsed && startDate.HasValue && endDateParsed && endDate.HasValue)
            {
                if (!(startDate.Value < endDate.Value))
                    yield return new ValidationResult("End Date must be after Start Date.", new[] { "EndDate" });
                else if (TypeId == GoalTypeEnum.Percent)
                    yield return new ValidationResult("Only absolute goals are supported with custom date ranges.", new[] { "TypeId" });
            }
        }

        // e.g. "Reach 1,000 Leads", "Reach $1,000 Spend", "Increase Leads 10.5% (to 1,105)", "Increase Spend 10.5% (to $1,105)"
        public string TargetFormattedBasedOn(DateRangeSummary rangeSummary, bool includeTimeframe = false)
        {
            if (TypeId == GoalTypeEnum.Absolute)
                return "Reach " + TargetFormatted + " " + this.MetricId + (includeTimeframe ? TimeframeFormatted(true, true) : "");
            else // Percent
                return "Increase " + this.MetricId + " " + TargetFormatted + " (to " + FormatSomeTarget(TargetBasedOn(rangeSummary)) + ")";
        }

        public string TimeframeFormatted(bool includeLeadingSpace = false, bool includeParentheses = false)
        {
            if (IsMonthly) return "";

            string prefix = (includeLeadingSpace ? " " : "") + (includeParentheses ? "(" : "");
            return prefix + StartDate + " - " + EndDate + (includeParentheses ? ")" : "");
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
                    return rangeSummary.Clicks.Value;
                case MetricEnum.Leads:
                    return rangeSummary.Conversions.Value;
                case MetricEnum.Spend:
                    return rangeSummary.Revenue.Value;
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