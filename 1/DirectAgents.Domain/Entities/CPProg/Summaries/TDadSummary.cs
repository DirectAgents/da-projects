using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    /// DailySummary for a "TD ad"
    public class TDadSummary : DatedStatsSummary
    {
        public int TDadId { get; set; }
        public virtual TDad TDad { get; set; }

        public virtual List<TDadSummaryMetric> Metrics { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && (Metrics == null || !Metrics.Any());
        }

        [NotMapped]
        public string TDadName { get; set; }

        [NotMapped]
        public string TDadEid { get; set; } // external id

        [NotMapped]
        public string StrategyName { get; set; }

        [NotMapped]
        public string StrategyEid { get; set; }

        [NotMapped]
        public string AdSetName { get; set; }

        [NotMapped]
        public string AdSetEid { get; set; }

        [NotMapped]
        public IEnumerable<TDadExternalId> ExternalIds { get; set; }

        [NotMapped]
        public string StrategyType { get; set; }

        [NotMapped]
        public string StrategyTargetingType { get; set; }

        [NotMapped]
        public int Width { get; set; }
    }
}