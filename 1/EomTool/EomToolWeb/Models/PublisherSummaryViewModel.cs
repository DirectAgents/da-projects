using System.Collections.Generic;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PublisherSummaryViewModel
    {
        public IEnumerable<PublisherSummary> PublisherSummaries { get; set; }
        public string Mode { get; set; }
        public bool IncludeZero { get; set; }
        public bool ShowActions
        {
            get
            {
                return string.IsNullOrWhiteSpace(Mode) || Mode == "held";
            }
        }
    }
}