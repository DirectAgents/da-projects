using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Contexts
{
    public class DATDContext : DbContext
    {
        const string adrollSchema = "adr";
        const string dbmSchema = "dbm";
        const string tdSchema = "td";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // TD
            modelBuilder.Entity<Platform>().ToTable("Platform", tdSchema);
            modelBuilder.Entity<Account>().ToTable("Account", tdSchema);
            modelBuilder.Entity<DailySummary>()
                .HasKey(ds => new { ds.Date, ds.AccountId })
                .ToTable("DailySummary", tdSchema);
            modelBuilder.Entity<DailySummary>()
                .Property(t => t.Cost).HasPrecision(18, 6);

            // AdRoll
            modelBuilder.Entity<Advertisable>().ToTable("Advertisable", adrollSchema);
            modelBuilder.Entity<Ad>().ToTable("Ad", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .HasKey(ds => new { ds.Date, ds.AdId })
                .ToTable("AdDailySummary", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .Property(ds => ds.Cost).HasPrecision(18, 6);

            // DBM
            modelBuilder.Entity<InsertionOrder>().ToTable("InsertionOrder", dbmSchema);
            modelBuilder.Entity<Creative>().ToTable("Creative", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .HasKey(cds => new { cds.Date, cds.CreativeID })
                .ToTable("CreativeDailySummary", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .Property(cds => cds.Revenue).HasPrecision(18, 6);
        }

        // TD
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }

        // AdRoll
        public DbSet<Advertisable> Advertisables { get; set; }
        public DbSet<Ad> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdRollAdDailySummaries { get; set; }

        // DBM
        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<CreativeDailySummary> CreativeDailySummaries { get; set; }
    }
}
