using System;

namespace DirectAgents.Domain.Entities.TD
{
    public class BasicStat
    {
        public int Day { get; set; }
        public DateTime Date { get; set; } // also used for End Date
        public DateTime StartDate { get; set; } // also used for Start Day of Week

        public string DayName
        {
            get { return Enum.GetName(typeof(DayOfWeek), Day - 1); }
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }

        public decimal MediaSpend { get; set; }
        public decimal MgmtFee { get; set; }
        public double Budget { get; set; }
        public double Pacing { get; set; }

        public double CTR { get; set; }
        public double CR { get; set; }
        public double eCPC { get; set; }
        public double eCPA { get; set; }

        // For Strategy Stats...
        public int StrategyId { get; set; }
        public string StrategyName { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public bool ShowClickAndViewConv { get; set; }

        public void ComputeCalculatedStats()
        {
            CTR = (Impressions == 0) ? 0 : (double)Clicks / Impressions;
            CR = (Clicks == 0) ? 0 : (double)Conversions / Clicks;
            eCPC = (Clicks == 0) ? 0 : (double)(MediaSpend / Clicks);
            eCPA = (Conversions == 0) ? 0 : (double)(MediaSpend / Conversions);
            Pacing = (Budget == 0) ? 0 : (double)MediaSpend / Budget;
        }

        public void ComputeWeekStartDate(DayOfWeek startDayOfWeek = DayOfWeek.Monday)
        {
            StartDate = Date;
            while (StartDate.DayOfWeek != startDayOfWeek)
                StartDate = StartDate.AddDays(-1);
        }
    }
}
