using System.Data.Entity;

namespace LTWeb.DataAccess
{
    public class LTWebDataContext : DbContext
    {
        public LTWebDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<ServiceConfig> ServiceConfigs { get; set; }

        public DbSet<Lead> Leads { get; set; }

        public DbSet<AdminSetting> AdminSettings { get; set; }

        public DbSet<LeadPost> LeadPosts { get; set; }

        public LTWebDataContext()
        {
            //var initializer = new MyDBInitializer();
            //Database.SetInitializer(initializer);
        }
    }

    //public class MyDBInitializer : CreateDatabaseIfNotExists<LTWebDataContext>
    //{
    //    protected override void Seed(LTWebDataContext db)
    //    {
    //    }
    //}
}
