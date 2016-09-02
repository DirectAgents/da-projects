using System;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Models
{
    public class StatVM
    {
        public StatVM(DateTime date)
        {
            Date = date;
        }
        public StatVM(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public StatVM(SearchStat stat)
        {
            this.Add(stat);
        }
        public StatVM(BasicStat stat)
        {
            this.Add(stat);
        }

        // Designed to use one or the other: [Date] or [StartDate and EndDate]
        public DateTime Date { get; set; }

        public DateTime? _startDate;
        public DateTime StartDate
        {
            set { _startDate = value; }
            get { if (_startDate.HasValue) return _startDate.Value; else return Date; }
        }
        public DateTime EndDate
        {
            set { Date = value; }
            get { return Date; }
        }

        public string Name { get; set; }

        public decimal Spend { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Convs { get; set; }

        public double CTR
        {
            get { return (Impressions == 0) ? 0 : Math.Round((double)Clicks / Impressions, 4); }
        }
        public double ConvRate
        {
            get { return (Clicks == 0) ? 0 : Math.Round((double)Convs / Clicks, 4); }
        }

        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Spend / Clicks, 4); }
        }
        public decimal CPA
        {
            get { return (Convs == 0) ? 0 : Math.Round(Spend / Convs, 4); }
        }

        public void Add(SearchDailySummary sds)
        {
            Spend += sds.Cost;
            Impressions += sds.Impressions;
            Clicks += sds.Clicks;
            Convs += sds.Orders;
        }
        public void Add(SearchStat searchStat)
        {
            Spend += searchStat.Cost;
            Impressions += searchStat.Impressions;
            Clicks += searchStat.Clicks;
            Convs += searchStat.TotalLeads;
        }
        public void Add(BasicStat basicStat)
        {
            Spend += basicStat.MediaSpend;
            Impressions += basicStat.Impressions;
            Clicks += basicStat.Clicks;
            Convs += basicStat.Conversions;
        }
    }
}