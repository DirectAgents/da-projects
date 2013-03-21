using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Web.Models
{
    public class GoalVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Offer { get; set; }

        [Required]
        public string Metric { get; set; }

        [Required]
        public decimal Target { get; set; }
    }
}