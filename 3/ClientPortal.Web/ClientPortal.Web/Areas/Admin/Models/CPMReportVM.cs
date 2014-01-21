﻿using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class CPMReportVM
    {
        private CPMReport Model { get; set; }

        public CPMReportVM(CPMReport cpmReport)
        {
            this.Model = cpmReport;
        }

        public IEnumerable<CampaignDropVM> CampaignDropsOrdered
        {
            get { return Model.CampaignDropsOrdered.Select(cd => new CampaignDropVM(cd)); }
        }

        public int CPMReportId
        {
            get { return Model.CPMReportId; }
        }
        public string Summary
        {
            get { return ReplaceSpecialChars(Model.Summary); }
        }
        public string Conclusion
        {
            get { return ReplaceSpecialChars(Model.Conclusion); }
        }

        public string TotalVolume
        {
            get { return Model.TotalVolume.ToString("N0"); }
        }
        public string TotalOpens
        {
            get { return Model.TotalOpens.ToString("N0"); }
        }
        public string TotalClicks
        {
            get { return Model.TotalClicks.ToString("N0"); }
        }
        public string TotalLeads
        {
            get { return Model.TotalLeads.ToString("N0"); }
        }
        public string TotalCost
        {
            get { return Model.TotalCost.ToString("C2"); }
        }
        public string ClickThroughRate
        {
            get { return String.Format("{0:0.00%}", Model.ClickThroughRate); }
        }
        public string OpenRate
        {
            get { return String.Format("{0:0.00%}", Model.OpenRate); }
        }
        public string CostPerLead
        {
            get { return Model.CostPerLead.ToString("C2"); }
        }
        public string ConversionRate
        {
            get { return String.Format("{0:0.00%}", Model.ConversionRate); }
        }

        private string ReplaceSpecialChars(string text)
        {
            string pattern = @"^>>(.*)$";
            string replacement = "<li>$1</li>";
            string result = Regex.Replace(text, pattern, replacement, RegexOptions.Multiline);
            return result;
        }
    }
}