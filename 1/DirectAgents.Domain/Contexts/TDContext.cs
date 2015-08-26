using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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
            //modelBuilder.HasDefaultSchema(tdSchema); //can't do this b/c __MigrationHistory table is under dbo schema

            // TD
            modelBuilder.Entity<Advertiser>().ToTable("Advertiser", tdSchema);
            modelBuilder.Entity<Campaign>().ToTable("Campaign", tdSchema);
            modelBuilder.Entity<BudgetInfo>().ToTable("BudgetInfo", tdSchema);
            modelBuilder.Entity<Platform>().ToTable("Platform", tdSchema);
            modelBuilder.Entity<Account>().ToTable("Account", tdSchema);
            modelBuilder.Entity<DailySummary>().ToTable("DailySummary", tdSchema);
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudget.MediaSpend).HasPrecision(14, 2).HasColumnName("MediaSpend");
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudget.MgmtFeePct).HasPrecision(8, 3).HasColumnName("MgmtFeePct");
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudget.MarginPct).HasPrecision(8, 3).HasColumnName("MarginPct");
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MediaSpend).HasPrecision(14, 2);
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MgmtFeePct).HasPrecision(8, 3);
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MarginPct).HasPrecision(8, 3);
            modelBuilder.Entity<BudgetInfo>()
                .HasKey(b => new { b.CampaignId, b.Date });
            modelBuilder.Entity<DailySummary>()
                .HasKey(ds => new { ds.Date, ds.AccountId })
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
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<BudgetInfo> BudgetInfos { get; set; }
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
        public DbSet<CreativeDailySummary> DBMCreativeDailySummaries { get; set; }
    }
}
