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

            modelBuilder.Entity<DailySummary>()
                .HasKey(ds => new {ds.Date, ds.InsertionOrderID})
                .ToTable("DailySummary", dbmSchema);
        }

        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
    }
}
