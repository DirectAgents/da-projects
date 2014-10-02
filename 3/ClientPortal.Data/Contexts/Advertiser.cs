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
            this.StartDayOfWeek = 1;
            this.AdvertiserContacts = new HashSet<AdvertiserContact>();
            this.FileUploads = new HashSet<FileUpload>();
            this.Goals = new HashSet<Goal>();
            this.SearchCampaigns = new HashSet<SearchCampaign>();
            this.SearchAccounts = new HashSet<SearchAccount>();
        }
    
        public int AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }
        public byte[] Logo { get; set; }
        public string Culture { get; set; }
        public bool ShowCPMRep { get; set; }
        public bool ShowConversionData { get; set; }
        public string ConversionValueName { get; set; }
        public bool ConversionValueIsNumber { get; set; }
        public bool HasSearch { get; set; }
        public bool AutomatedReportsEnabled { get; set; }
        public string AutomatedReportsDestinationEmail { get; set; }
        public int AutomatedReportsPeriodDays { get; set; }
        public Nullable<System.DateTime> AutomatedReportsNextSendAfter { get; set; }
        public string AdWordsAccountId { get; set; }
        public string BingAdsAccountId { get; set; }
        public string AnalyticsProfileId { get; set; }
        public bool ShowSearchChannels { get; set; }
        public Nullable<int> AccountManagerId { get; set; }
        public Nullable<System.DateTime> LatestDaySums { get; set; }
        public Nullable<System.DateTime> LatestClicks { get; set; }
        public int StartDayOfWeek { get; set; }
    
        public virtual ICollection<AdvertiserContact> AdvertiserContacts { get; set; }
        public virtual ICollection<FileUpload> FileUploads { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<SearchCampaign> SearchCampaigns { get; set; }
        public virtual ICollection<SearchAccount> SearchAccounts { get; set; }
        public virtual CakeContact AccountManager { get; set; }
    }
}
