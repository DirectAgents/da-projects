﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmLineItem : DbmBaseEntity
    {
        public int? InsertionOrderId { get; set; }

        [ForeignKey("InsertionOrderId")]
        public virtual DbmInsertionOrder InsertionOrder { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
