using ClientPortal.Data.Entities.TD.AdRoll;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClientPortal.Data.Entities.TD
{
    public class TDContext : DbContext
    {
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
            modelBuilder.Entity<AdDailySummary>()
                .Property(ds => ds.Spend).HasPrecision(18, 6);

            //Trading Desk - general
            modelBuilder.Entity<TradingDeskAccount>()
                .Property(m => m.FixedMetricValue).HasPrecision(18, 6);
        }

        //AdRoll
        public DbSet<AdRollProfile> AdRollProfiles { get; set; }
        public DbSet<AdRollAd> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdDailySummaries { get; set; }

        //General
        public DbSet<TradingDeskAccount> TradingDeskAccounts { get; set; }
    }
}
