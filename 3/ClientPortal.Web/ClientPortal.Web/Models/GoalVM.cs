using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
        [Required]
        public string Name { get; set; }

        [Display(Name="Type")]
        public GoalTypeEnum TypeId { get; set; }

        [Required]
        public string Offer { get; set; }

        [Display(Name="Metric")]
        public MetricEnum MetricId { get; set; }

        [Required]
        public decimal Target { get; set; }

        public GoalVM(Goal goal)
        {
            this.Name = goal.Name;
            this.TypeId = goal.TypeId;
            this.Offer = goal.OfferId != null ? goal.OfferId.ToString() : "";
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