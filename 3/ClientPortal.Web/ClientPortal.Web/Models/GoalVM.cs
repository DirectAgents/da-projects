using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
        [Required]
        public string Name { get; set; }

        public GoalTypeVM Type { get; set; }
        public GoalTypeEnum TypeId { get; set; }

        [Required]
        public string Offer { get; set; }

        public MetricVM Metric { get; set; }
        public MetricEnum MetricId { get; set; }

        [Required]
        public decimal Target { get; set; }
    }

    public enum GoalTypeEnum { Absolute = 1, Percent };
    public enum MetricEnum { Conversions = 1, Revenue };

    public class GoalTypeVM
    {
        public GoalTypeEnum Id { get; set; }
        public string Name { get; set; }
    }

    public class MetricVM
    {
        public MetricEnum Id { get; set; }
        public string Name { get; set; }
    }

}