﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.DBM
{
    public class Creative
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }

        public int InsertionOrderID { get; set; }
        public virtual InsertionOrder InsertionOrder { get; set; }
    }
}
