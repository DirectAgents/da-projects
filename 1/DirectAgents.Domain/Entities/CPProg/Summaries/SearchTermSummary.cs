using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    /// DailySummary for a SearchTerm
    public class SearchTermSummary : DatedStatsSummaryWithRev
    {
        public int SearchTermId { get; set; }

        public virtual SearchTerm SearchTerm { get; set; }

        public virtual List<SearchTermSummaryMetric> Metrics { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && (Metrics == null || !Metrics.Any());
        }

        [NotMapped]
        public string KeywordName { get; set; }

        [NotMapped]
        public string KeywordEid { get; set; }

        [NotMapped]
        public string AdSetName { get; set; }

        [NotMapped]
        public string AdSetEid { get; set; }

        [NotMapped]
        public string StrategyName { get; set; }

        [NotMapped]
        public string StrategyEid { get; set; }

        [NotMapped]
        public string StrategyType { get; set; }

        [NotMapped]
        public string StrategyTargetingType { get; set; }

        [NotMapped]
        public string SearchTermName { get; set; }

        [NotMapped]
        public string SearchTermMediaType { get; set; }
    }
}