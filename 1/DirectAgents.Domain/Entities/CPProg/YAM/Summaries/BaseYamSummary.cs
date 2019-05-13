using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public abstract class BaseYamSummary
    {
        [Key]
        public int EntityId { get; set; }

        [Key]
        public DateTime Date { get; set; }

        public int Impressions { get; set; }

        public int Clicks { get; set; }

        public int ClickThroughConversion { get; set; }

        public int ViewThroughConversion { get; set; }

        public decimal ConversionValue { get; set; }

        public decimal AdvertiserSpending { get; set; }

        public decimal? ClickConversionValueByPixelQuery { get; set; }

        public decimal? ViewConversionValueByPixelQuery { get; set; }

        public virtual void SetStats(IEnumerable<BaseYamSummary> stats)
        {
            Impressions = stats.Sum(x => x.Impressions);
            Clicks = stats.Sum(x => x.Clicks);
            ClickThroughConversion = stats.Sum(x => x.ClickThroughConversion);
            ViewThroughConversion = stats.Sum(x => x.ViewThroughConversion);
            ConversionValue = stats.Sum(x => x.ConversionValue);
            AdvertiserSpending = stats.Sum(x => x.AdvertiserSpending);
        }

        public virtual bool IsEmpty()
        {
            const decimal emptyValue = default(decimal);
            return Impressions == emptyValue
                   && Clicks == emptyValue
                   && ClickThroughConversion == emptyValue
                   && ViewThroughConversion == emptyValue
                   && ConversionValue == emptyValue
                   && AdvertiserSpending == emptyValue
                   && (!ClickConversionValueByPixelQuery.HasValue || ClickConversionValueByPixelQuery == emptyValue)
                   && (!ViewConversionValueByPixelQuery.HasValue || ViewConversionValueByPixelQuery == emptyValue);
        }
    }
}
