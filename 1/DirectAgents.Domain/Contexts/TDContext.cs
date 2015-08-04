using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;

namespace DirectAgents.Domain.Contexts
{
    public class DATDContext : DbContext
    {
        const string adrollSchema = "adr";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // AdRoll
            modelBuilder.Entity<Advertisable>().ToTable("Advertisable", adrollSchema);
            modelBuilder.Entity<AdvertisableStat>()
                .HasKey(t => new { t.Date, t.AdvertisableId })
                .ToTable("AdvertisableStat", adrollSchema);
            modelBuilder.Entity<AdvertisableStat>()
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<Ad>().ToTable("Ad", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .HasKey(ds => new { ds.Date, ds.AdId })
                .ToTable("AdDailySummary", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .Property(ds => ds.Cost).HasPrecision(18, 6);
        }

        public DbSet<Advertisable> Advertisables { get; set; }
        public DbSet<AdvertisableStat> AdvertisableStats { get; set; }
        public DbSet<Ad> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdRollAdDailySummaries { get; set; }
    }
}
