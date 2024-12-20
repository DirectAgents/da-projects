﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    /// DailySummary for an AdSet
    public class AdSetSummary : DatedStatsSummaryWithRev
    {
        public int AdSetId { get; set; }
        public virtual AdSet AdSet { get; set; }

        public virtual List<AdSetSummaryMetric> Metrics { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && (Metrics == null || !Metrics.Any());
        }

        [NotMapped]
        public string AdSetName { get; set; }
        [NotMapped]
        public string AdSetEid { get; set; } // external id

        [NotMapped]
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; } // external id
        [NotMapped]
        public string StrategyType { get; set; }
    }
}