using System;
using System.Collections.Generic;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Web.Areas.AB.Models
{
    public class DashboardVM
    {
        public ABClient ABClient { get; set; }

        // Use one or the other...
        public IEnumerable<ABStat> ABStats { get; set; }
        public IEnumerable<ABLineItem> LineItems { get; set; }
    }

    //TODO: what should this be called? (This is the main view for a single client.)
    public class DetailVM
    {
        public ABClient ABClient { get; set; }
        public IEnumerable<LineItemGroup> MonthGroups { get; set; }
    }

    public class LineItemGroup
    {
        public DateTime Month { get; set; }
        public IEnumerable<ABLineItem> LineItems { get; set; }

        //public LineItemGroup(DateTime month, IEnumerable<IRTLineItem> lineItems)
        //{
        //    this.Month = month;
        //    var liList = new List<ABLineItem>();
        //    foreach (var rtLI in lineItems)
        //    {
        //        var abLI = new ABLineItem(rtLI);
        //        liList.Add(abLI);
        //    }
        //    this.LineItems = liList;
        //}
    }
}