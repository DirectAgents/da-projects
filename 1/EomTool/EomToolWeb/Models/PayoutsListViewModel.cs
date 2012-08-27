using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PayoutsListViewModel
    {
        public IEnumerable<PublisherPayout> Payouts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public bool ShowActions { get; set; }
        public bool TestMode { get; set; }
    }
}