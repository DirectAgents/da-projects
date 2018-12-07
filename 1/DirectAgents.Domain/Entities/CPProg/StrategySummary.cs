using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class StrategySummary : DatedStatsSummaryWithRev
    {
        public int StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }
        public virtual List<StrategySummaryMetric> Metrics { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && (Metrics == null || !Metrics.Any());
        }

        [NotMapped]
        public string StrategyName { get; set; }

        [NotMapped]
        public string StrategyEid { get; set; } // external id

        [NotMapped]
        public string StrategyType { get; set; }

        [NotMapped]
        public string CampaignType { get; set; }
    }
}