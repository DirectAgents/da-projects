using ClientPortal.Data.Entities.TD.DBM;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClientPortal.Data.Entities.TD
{
    public class TDContext : DbContext
    {
        const string dbmSchema = "dbm";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<InsertionOrder>().ToTable("InsertionOrder", dbmSchema);
            modelBuilder.Entity<Creative>().ToTable("Creative", dbmSchema);

            modelBuilder.Entity<DBMDailySummary>()
                .HasKey(ds => new {ds.Date, ds.InsertionOrderID})
                .ToTable("DailySummary", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .HasKey(cds => new {cds.Date, cds.CreativeID})
                .ToTable("CreativeDailySummary", dbmSchema);

            modelBuilder.Entity<TradingDeskAccount>().Property(m => m.FixedMetricValue).HasPrecision(18, 6);
        }

        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<DBMDailySummary> DBMDailySummaries { get; set; }
        public DbSet<CreativeDailySummary> CreativeDailySummaries { get; set; }

        public DbSet<TradingDeskAccount> TradingDeskAccounts { get; set; }
    }
}
