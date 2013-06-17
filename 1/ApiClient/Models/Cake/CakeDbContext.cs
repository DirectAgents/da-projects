using System.Data.Entity;

namespace ApiClient.Models.Cake
{
    public class CakeDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<conversion>()
            //    .HasKey(c => c.conversion_id)
            //    .ToTable("Conversion");

            modelBuilder.Entity<DailySummary>()
                        .ToTable("DailySummary");
        }

        //public DbSet<conversion> Conversions { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
    }
}
