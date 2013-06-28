using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPortal.Data.DTOs
{
    public class SearchStat
    {
        public string Title { get; set; }
        public string Range { get; set; }
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }

        public int ROAS
        {
            get { return (int)Math.Round(100 * Revenue / Cost); }
        }
        public decimal CPO
        {
            get { return Math.Round(Cost / Orders, 2); }
        }

        public decimal Days { get; set; }

        public SearchStat(bool isWeekly, int month, int day, int impressions, int clicks, int orders, decimal revenue, decimal cost, string title = null)
        {
            this.Date = new DateTime(2013, month, day);
            this.Range = ToRangeName(this.Date, isWeekly);
            this.Title = title ?? Range;

            if (isWeekly)
                Days = 7;
            else
                Days = day;

            this.Impressions = impressions;
            this.Clicks = clicks;
            this.Orders = orders;
            this.Revenue = revenue;
            this.Cost = cost;
        }

        private string ToRangeName(DateTime date, bool isWeekly)
        {
            if (isWeekly)
            {
                DateTime weekStart = date.AddDays(-6);
                return weekStart.ToString("M/d") + " - " + date.ToString("M/d");
            }
            else
                return date.ToString("MMM-yy");
        }
    }
}
