﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DirectAgents.Domain.Entities.Cake
{
    public class DailySummary
    {
        public virtual Campaign Campaign { get; set; }

        [Key, Column(Order=0)]
        [ForeignKey("Campaign")]
        public int Pid { get; set; }

        [Key, Column(Order=1)]
        public System.DateTime Date { get; set; }

        public int Clicks { get; set; }

        public int Conversions { get; set; }

        public int Paid { get; set; }

        public int Sellable { get; set; }

        public decimal Cost { get; set; }

        public decimal Revenue { get; set; }
    }
}
