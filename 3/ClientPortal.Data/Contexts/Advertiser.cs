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
    
    public partial class Advertiser
    {
        public Advertiser()
        {
            this.AdvertiserContacts = new HashSet<AdvertiserContact>();
            this.FileUploads = new HashSet<FileUpload>();
            this.Goals = new HashSet<Goal>();
            this.ScheduledReports = new HashSet<ScheduledReport>();
        }
    
        public int AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }
        public byte[] Logo { get; set; }
        public string Culture { get; set; }
        public bool ShowCPMRep { get; set; }
        public bool ShowConversionData { get; set; }
        public string ConversionValueName { get; set; }
        public bool ConversionValueIsNumber { get; set; }
    
        public virtual ICollection<AdvertiserContact> AdvertiserContacts { get; set; }
        public virtual ICollection<FileUpload> FileUploads { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<ScheduledReport> ScheduledReports { get; set; }
    }
}
