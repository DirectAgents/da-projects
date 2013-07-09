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
            get { return Cost == 0 ? 0 : (int)Math.Round(100 * Revenue / Cost); }
        }
        public decimal CPO
        {
            get { return Orders == 0 ? 0 : Math.Round(Cost / Orders, 2); }
        }

        public decimal Days { get; set; }

        //public int Year
        //{
        //    set { this.Date = new DateTime(value, 1, 1); }
        //}
        //public int Month // (note: set year first)
        //{
        //    set
        //    {
        //        this.Date = new DateTime(this.Date.Year, value, 1).AddMonths(1).AddDays(-1);
        //        this.Days = this.Date.Day;

        //        this.Range = ToRangeName(this.Date, false);
        //        this.Title = Range;
        //    }
        //}

        // --- Initializers (monthly, weekly or custom date range) ---

        public DateTime MonthByMaxDate
        {
            set
            {
                this.Date = value;
                this.Days = this.Date.Day;

                this.Range = ToRangeName(this.Date, false);
                this.Title = Range;
            }
        }

        public DateTime WeekByMaxDate
        {
            set
            {
                var monday = value;
                while (monday.DayOfWeek != DayOfWeek.Monday)
                    monday = monday.AddDays(-1);

                this.Days = (value - monday).Days + 1;
                //this.Date = monday.AddDays(6);
                this.Date = value;

                this.Range = ToRangeName(monday, this.Date);
                this.Title = Range;
            }
        }

        //public int Week
        //{
        //    set
        //    {
        //        this.Days = 7;

        //        this.Date = new DateTime(2013, 1, 1).AddDays(value * 7);
        //        while (this.Date.DayOfWeek != DayOfWeek.Sunday)
        //            this.Date = this.Date.AddDays(-1);

        //        this.Range = ToRangeName(this.Date, true);
        //        this.Title = Range;
        //    }
        //}

        public DateTime CustomByStartDate // (note: set (end) Date first)
        {
            set
            {
                if (value > this.Date)
                    throw new Exception("Must set (end) Date first and start date must be <= end date.");

                this.Days = (this.Date - value).Days + 1;
                this.Range = ToRangeName(value, this.Date);
            }
        }

        // --- Constructors ---

        public SearchStat() { }

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

        // --- Private methods ---

        private string ToRangeName(DateTime date, bool isWeekly)
        {
            if (isWeekly)
            {
                DateTime weekStart = date.AddDays(-6);
                return ToRangeName(weekStart, date);
            }
            else
                return date.ToString("MMM-yy");
        }

        private string ToRangeName(DateTime start, DateTime end)
        {
            return start.ToString("M/d") + " - " + end.ToString("M/d");
        }
    }
}
