using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.AB
{
    public class AcctPayment
    {
        public int Id { get; set; }
        public int AcctId { get; set; }
        [ForeignKey("AcctId")]
        public virtual ClientAccount ClientAccount { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<AcctPaymentBit> Bits { get; set; }
    }
}
