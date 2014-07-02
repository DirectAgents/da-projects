using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class Affiliate
    {
        [NotMapped]
        public Affiliate PreviousMonthAffiliate { get; set; }
    }
}
