﻿using System.Collections.Generic;

namespace DirectAgents.Domain.Entities.AB
{
    public class ClientAccount
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual ABClient Client { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProtoCampaign> ProtoCampaigns { get; set; }
        public virtual ICollection<ProtoPaymentBit> ProtoPaymentBits { get; set; }
    }

}
