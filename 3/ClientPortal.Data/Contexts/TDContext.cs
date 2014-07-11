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

            modelBuilder.Entity<DailySummary>()
                .HasKey(ds => new {ds.Date, ds.InsertionOrderID})
                .ToTable("DailySummary", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .HasKey(cds => new {cds.Date, cds.CreativeID})
                .ToTable("CreativeDailySummary", dbmSchema);
        }

        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<CreativeDailySummary> CreativeDailySummaries { get; set; }
    }
}
