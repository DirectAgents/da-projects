using System.Data.Entity;

namespace LTWeb.DataAccess
{
    public class LTWebDataContext : DbContext
    {
        public DbSet<ServiceConfig> ServiceConfigs { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<AdminSetting> AdminSettings { get; set; }

        public LTWebDataContext()
        {
#if(DEBUG)
            var initializer = new DropCreateDatabaseIfModelChanges<LTWebDataContext>();
            Database.SetInitializer(initializer);
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // todo: do things with modelBuilder like:
            //      modelBuilder.Entity<Bid>()
            //          .HasRequired(x => x.Auction)
            //          .WithMany()
            //          .WillCascadeOnDelete(false);
        }
    }
}
