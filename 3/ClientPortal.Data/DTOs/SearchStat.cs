using System;

namespace ClientPortal.Data.DTOs
{
    public class SimpleSearchStat
    {
        public string Title { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public int ROAS { get; set; }
        public decimal Margin { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Orders { get; set; }
        public decimal CPO { get; set; }

        public int Calls { get; set; }
    }

    public class SearchStat
    {
        public string Campaign { get; set; }
        public string Channel { get; set; }
        public string Title { get; set; }
        public string Range { get; set; }

        private string _network;
        public string Network
        {
            set { _network = value; }
            get
            {
                switch (_network)
                {
                    case "S":
                        return "Search";
                    case "D":
                        return "Display";
                    default:
                        return _network;
                }
            }
        }
        private string _device;
        public string Device
        {
            set { _device = value; }
            get
            {
                switch (_device)
                {
                    case "C":
                        return "Computer";
                    case "M":
                        return "Mobile";
                    case "T":
                        return "Tablet";
                    default:
                        return _device;
                }
            }
        }
        private string _clickType;
        public string ClickType
        {
            set { _clickType = value; }
            get
            {
                switch (_clickType)
                {
                    case "H":
                        return "Headline";
                    case "S":
                        return "Sitelink";
                    case "O":
                        return "Offer";
                    default:
                        return _clickType;
                }
            }
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }

        public int Calls { get; set; }

        //public int ROI
        //{
        //    get { return Cost == 0 ? 0 : (int)Math.Round(100 * Revenue / Cost) - 100; }
        //}
        public int ROAS
        {
            get { return Cost == 0 ? 0 : (int)Math.Round(100 * Revenue / Cost); }
        }
        public decimal Margin
        {
            get { return Revenue - Cost; }
        }
        public decimal CPO
        {
            get { return Orders == 0 ? 0 : Math.Round(Cost / Orders, 2); }
        }
        public decimal OrderRate
        {
            get { return Clicks == 0 ? 0 : Math.Round((decimal)100 * Orders / Clicks, 2); }
        }
        public decimal RevenuePerOrder
        {
            get { return Orders == 0 ? 0 : Math.Round(Revenue / Orders, 2); }
        }
        public decimal CPC
        {
            get { return Clicks == 0 ? 0 : Math.Round(Cost / Clicks, 2); }
        }
        public decimal CTR
        {
            get { return Impressions == 0 ? 0 : Math.Round((decimal)100 * Clicks / Impressions, 2); }
        }
        public decimal OrdersPerDay
        {
            get { return Days == 0 ? 0 : Math.Round((decimal)Orders / Days, 2); }
        }

        public int Days { get; set; }

        public DateTime StartDate
        {
            get { return EndDate.AddDays((Days - 1) * -1); }
        }

        private DayOfWeek WeekEndDay
        {
            get
            {
                if (this.WeekStartDay == DayOfWeek.Sunday)
                    return DayOfWeek.Saturday;
                else
                    return this.WeekStartDay - 1;
            }
        }

        // --- Pre-initializers ---

        // Set this first - unless using default value
        internal DayOfWeek WeekStartDay {
            set { _weekStartDay = value; }
            get { return _weekStartDay; }
        }
        private DayOfWeek _weekStartDay = DayOfWeek.Monday;

        // Set this next, if required by initializer
        public DateTime EndDate { get; set; }

        // Optionally, set this to "latest" (yesterday or today) - to fill in holes in dateranges
        internal DateTime? FillToLatest { get; set; } // TODO: implement for other than WeekByMaxDate

        // Then use one of the following...

        // --- Initializers (monthly, weekly or custom date range) ---

        internal DateTime MonthByMaxDate
        {
            set
            {
                this.EndDate = value;
                this.Days = this.EndDate.Day;

                this.Range = ToRangeName(this.EndDate, false, true);
                this.Title = ToRangeName(this.EndDate, false);
            }
        }

        internal DateTime WeekByMaxDate
        {
            set
            {
                var startDate = value;
                // Stretch back until we reach a WeekStartDay
                while (startDate.DayOfWeek != this.WeekStartDay)
                    startDate = startDate.AddDays(-1);

                this.EndDate = value;
                if (this.FillToLatest.HasValue)
                {
                    // Stretch forward until we reach a WeekEndDay (or FillToLatest, whichever comes first)
                    var weekEndDay = this.WeekEndDay;
                    while (this.EndDate < this.FillToLatest.Value && this.EndDate.DayOfWeek != weekEndDay)
                        this.EndDate = this.EndDate.AddDays(1);
                }
                this.Days = (this.EndDate - startDate).Days + 1;

                this.Range = ToRangeName(startDate, this.EndDate, true);
                this.Title = ToRangeName(startDate, this.EndDate);
            }
        }

        internal DateTime CustomByStartDate // (note: set (end) Date first)
        {
            set
            {
                if (value > this.EndDate)
                    throw new Exception("Must set (end) Date first and start date must be <= end date.");

                this.Days = (this.EndDate - value).Days + 1;
                this.Range = ToRangeName(value, this.EndDate);
            }
        }

        // will set Title only if value is not null
        internal string TitleIfNotNull
        {
            set { if (value != null) this.Title = value; }
        }

        // --- Constructors ---

        // call this when using one of the above initializers
        public SearchStat() { }

        // use for one day's stats, or for a week- ending on the specified date
        public SearchStat(bool isWeekly, int year, int month, int day, int impressions, int clicks, int orders, decimal revenue, decimal cost, string title = null)
        {
            this.EndDate = new DateTime(year, month, day);
            this.Range = ToRangeName(this.EndDate, isWeekly, true);
            this.Title = title ?? ToRangeName(this.EndDate, isWeekly);

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

        private string ToRangeName(DateTime date, bool isWeekly, bool prependYMD = false)
        {
            if (isWeekly)
            {
                DateTime weekStart = date.AddDays(-6);
                return ToRangeName(weekStart, date, prependYMD);
            }
            else
                return (prependYMD ? date.ToString("yyyyMMdd") + " " : "") + date.ToString("MMM-yy");
        }

        private string ToRangeName(DateTime start, DateTime end, bool prependYMD = false)
        {
            return (prependYMD ? start.ToString("yyyyMMdd") + " " : "") + start.ToString("MM/dd") + " - " + end.ToString("MM/dd");
        }
    }
}
