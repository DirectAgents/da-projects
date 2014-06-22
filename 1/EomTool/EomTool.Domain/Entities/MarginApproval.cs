using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class MarginApproval
    {
        [NotMapped]
        public Campaign Campaign { get; set; }
        [NotMapped]
        public Affiliate Affiliate { get; set; }
    }
}
