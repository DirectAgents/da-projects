using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EomTool.Domain.Entities
{
    public partial class PaymentBatch
    {
        [NotMapped]
        public string AccountingPeriod { get; set; }

        [NotMapped]
        public IEnumerable<PublisherPayment> Payments { get; set; }
    }
}
