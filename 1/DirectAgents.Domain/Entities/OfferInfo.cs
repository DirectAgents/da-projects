using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectAgents.Domain.Entities
{
    //[Table("OfferInfo")]
    //public class OfferInfo : Offer
    //{
    //    public decimal Budget { get; set; }
    //    //public string Currency { get; set; }
    //}
    [Table("OfferInfo")]
    public class OfferInfo
    {
        [Key]
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        public decimal Budget { get; set; }
        public bool BudgetIsMonthly { get; set; }
        public DateTime? BudgetStart { get; set; }
    }
}
