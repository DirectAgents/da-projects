using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public GoalTypeEnum Type { get; set; }

        [Required]
        public string Offer { get; set; }

        [Required]
        public MetricTypeEnum Metric { get; set; }

        [Required]
        public decimal Target { get; set; }
    }

    public enum GoalTypeEnum { Absolute, Percent };
    public enum MetricTypeEnum { Conversions, Revenue };
/*
    public class GoalTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MetricTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
*/
}