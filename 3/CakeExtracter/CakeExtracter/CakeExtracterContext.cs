using System.Data.Entity;

namespace CakeMarketing
{
    public class CakeExtracterContext : DbContext
    {
        public DbSet<conversion> Conversions { get; set; }
        public DbSet<click> Clicks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<conversion>().HasKey(c => c.conversion_id);
            modelBuilder.Entity<click>().HasKey(c => c.click_id);
        }
    }

    //public partial class click
    //{
    //    public int Id { get; set; }
    //}
}
