using System.Data.Entity;

namespace ApiClient.Models
{
    public class CakeDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<conversion>()
                .HasKey(c => c.conversion_id);
        }

        public DbSet<conversion> Conversions { get; set; }
    }
}
