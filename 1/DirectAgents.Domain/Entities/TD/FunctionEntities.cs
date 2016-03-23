using System;

namespace DirectAgents.Domain.Entities.TD
{
    public class BasicStat
    {
        public int Day { get; set; }

        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal MediaSpend { get; set; }
        public decimal MgmtFee { get; set; }
        // Budget, Pacing

        public double CTR { get; set; }
        public double CR { get; set; }
        public double eCPC { get; set; }
        public double eCPA { get; set; }

        public void ComputeCalculatedStats()
        {
            CTR = (Impressions == 0) ? 0 : (double)Clicks / Impressions;
            CR = (Conversions == 0) ? 0 : (double)Conversions / Clicks;
            eCPC = (Clicks == 0) ? 0 : (double)(MediaSpend / Clicks);
            eCPA = (Conversions == 0) ? 0 : (double)(MediaSpend / Conversions);
        }
    }
}
