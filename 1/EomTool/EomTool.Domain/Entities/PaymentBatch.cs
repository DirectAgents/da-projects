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

        [NotMapped]
        public string approver_abbrev
        {
            get
            {
                if (approver_identity != null && approver_identity.StartsWith("DIRECTAGENTS\\"))
                    return (approver_identity.Substring(13));
                else return approver_identity;
            }
        }
    }
}
