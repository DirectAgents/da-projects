using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class PublisherPayment
    {
        [NotMapped]
        public string AccountingPeriod { get; set; }
    }
}
