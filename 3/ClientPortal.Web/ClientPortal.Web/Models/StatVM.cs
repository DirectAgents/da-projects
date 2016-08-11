using System;
using ClientPortal.Data.DTOs;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Models
{
    public class StatVM
    {
        public StatVM(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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