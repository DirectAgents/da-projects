//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientPortal.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampaignDrop
    {
        public CampaignDrop()
        {
            this.CreativeStats = new HashSet<CreativeStat>();
            this.CampaignDropCopies = new HashSet<CampaignDrop>();
            this.CPMReports = new HashSet<CPMReport>();
        }
    
        public int CampaignDropId { get; set; }
        public int CampaignId { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> Volume { get; set; }
        public Nullable<int> Opens { get; set; }
        public string Subject { get; set; }
        public Nullable<int> CopyOf { get; set; }
        public string FromEmail { get; set; }
        public bool CombineCreatives { get; set; }
    
        public virtual Campaign Campaign { get; set; }
        public virtual ICollection<CreativeStat> CreativeStats { get; set; }
        public virtual ICollection<CampaignDrop> CampaignDropCopies { get; set; }
        public virtual CampaignDrop CampaignDropOriginal { get; set; }
        public virtual ICollection<CPMReport> CPMReports { get; set; }
    }
}
