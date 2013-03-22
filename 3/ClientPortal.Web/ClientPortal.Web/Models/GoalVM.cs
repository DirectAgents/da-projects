using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
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

        public GoalVM(Goal goal, string offerName)
        {
            this.Name = goal.Name;
            this.TypeId = goal.TypeId;
            this.OfferId = goal.OfferId;
            this.OfferName = offerName;
            this.MetricId = goal.MetricId;
            this.Target = goal.Target;
        }

        public GoalVM()
        { // defaults
            this.TypeId = GoalTypeEnum.Absolute;
            this.MetricId = MetricEnum.Conversions;
        }
    }
}