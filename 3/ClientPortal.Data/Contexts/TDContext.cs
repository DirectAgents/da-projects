﻿using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClientPortal.Data.Entities.TD
{
    public class TDContext : DbContext
    {
        const string dbmSchema = "dbm";
        const string adrollSchema = "adr";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //AdRoll
            modelBuilder.Entity<AdRollProfile>().ToTable("AdRollProfile", adrollSchema);
            modelBuilder.Entity<AdRollAd>().ToTable("AdRollAd", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .HasKey(ds => new { ds.Date, ds.AdRollAdId })
                .ToTable("AdDailySummary", adrollSchema);

            //DBM
            modelBuilder.Entity<InsertionOrder>().ToTable("InsertionOrder", dbmSchema);
            modelBuilder.Entity<Creative>().ToTable("Creative", dbmSchema);
            modelBuilder.Entity<DBMDailySummary>()
                .HasKey(ds => new { ds.Date, ds.InsertionOrderID })
                .ToTable("DailySummary", dbmSchema);
            modelBuilder.Entity<DBMDailySummary>()
                .Property(ds => ds.Revenue).HasPrecision(18, 6);
            modelBuilder.Entity<CreativeDailySummary>()
                .HasKey(cds => new { cds.Date, cds.CreativeID })
                .ToTable("CreativeDailySummary", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .Property(cds => cds.Revenue).HasPrecision(18, 6);

            //Trading Desk - general
            modelBuilder.Entity<TradingDeskAccount>()
                .Property(m => m.FixedMetricValue).HasPrecision(18, 6);
        }

        public DbSet<AdRollProfile> AdRollProfiles { get; set; }
        public DbSet<AdRollAd> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdDailySummaries { get; set; }

        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<DBMDailySummary> DBMDailySummaries { get; set; }
        public DbSet<CreativeDailySummary> CreativeDailySummaries { get; set; }

        public DbSet<TradingDeskAccount> TradingDeskAccounts { get; set; }
    }

    //public class DailyStats
    //{
    //    public int Id { get; set; }
    //    public DateTime Date { get; set; }

    //    // AdRoll
    //    public int? AdRollAdId { get; set; }
    //    public virtual AdRollAd AdRollAd { get; set; }
    //    // DBM
    //    public int? CreativeID { get; set; }
    //    public virtual Creative Creative { get; set; }

    //    public int Impressions { get; set; }
    //    public int Clicks { get; set; }
    //    public int Conversions { get; set; }
    //    public decimal Revenue { get; set; }
    //}
}
