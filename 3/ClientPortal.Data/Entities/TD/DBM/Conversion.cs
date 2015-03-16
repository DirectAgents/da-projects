using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Entities.TD.DBM
{
    public class DBMConversion
    {
        public string AuctionID { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime? ViewTime { get; set; }
        public DateTime? RequestTime { get; set; }
        public int? InsertionOrderID { get; set; }
        public int? LineItemID { get; set; }
        public int? CreativeID { get; set; }
    }
}
