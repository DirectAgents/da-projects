using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class AffiliatesListViewModel
    {
        public IEnumerable<Affiliate> Affiliates { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}