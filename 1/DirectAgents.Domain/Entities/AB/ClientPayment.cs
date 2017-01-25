using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.AB
{
    public class ClientPayment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<ClientPaymentBit> Bits { get; set; }

        public decimal TotalValue()
        {
            if (Bits != null && Bits.Any())
                return Bits.Sum(b => b.Value);
            else
                return 0;
        }
    }

    public class ClientPaymentBit
    {
        public int Id { get; set; }
        public int ClientPaymentId { get; set; }
        public decimal Value { get; set; }
    }
}
