﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.AB
{
    public class AcctSpendBucket
    {
        public int Id { get; set; }
        public int AcctId { get; set; }
        [ForeignKey("AcctId")]
        public virtual ClientAccount ClientAccount { get; set; }
        public string Name { get; set; }

        public ICollection<AcctSpendBit> Bits { get; set; }
    }
}
