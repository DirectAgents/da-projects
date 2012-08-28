using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PublisherSummaryViewModel
    {
        public IEnumerable<PublisherSummary> PublisherSummaries { get; set; }
        public string Mode { get; set; }
    }
}