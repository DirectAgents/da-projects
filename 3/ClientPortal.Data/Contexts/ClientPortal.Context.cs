﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClientPortalContext : DbContext
    {
        public ClientPortalContext()
            : base("name=ClientPortalContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ConversionData> ConversionDatas { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<AdvertiserContact> AdvertiserContacts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<GeneratedReport> GeneratedReports { get; set; }
        public DbSet<ScheduledReportRecipient> ScheduledReportRecipients { get; set; }
        public DbSet<EmailedReport> EmailedReports { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<ScheduledReport> ScheduledReports { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<MetricCount> MetricCounts { get; set; }
        public DbSet<MetricValue> MetricValues { get; set; }
        public DbSet<SearchDailySummary> SearchDailySummaries { get; set; }
        public DbSet<Conversion> Conversions { get; set; }
        public DbSet<SearchCampaign> SearchCampaigns { get; set; }
        public DbSet<Offer> Offers { get; set; }
    }
}
