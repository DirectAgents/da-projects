using System;

namespace DirectAgents.Domain.Entities.AB
{
    public class ClientPayment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }

        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
