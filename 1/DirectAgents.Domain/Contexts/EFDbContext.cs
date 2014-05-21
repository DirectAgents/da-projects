using System.Data.Entity;
using DirectAgents.Domain.Entities.Wiki;
using DirectAgents.Domain.Entities.Cake;

namespace DirectAgents.Domain.Contexts
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base()
        {
//            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Disable the check to see if the model is out of sync with the database
//            Database.SetInitializer<EFDbContext>(null);

            string schema = "wiki";

            modelBuilder.Entity<Campaign>()
                .HasMany(u => u.AccountManagers)
                .WithMany(t => t.AccountManagerCampaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("PersonId");
                    x.ToTable("CampaignAccountManagers", schema);
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(u => u.AdManagers)
                .WithMany(t => t.AdManagerCampaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("PersonId");
                    x.ToTable("CampaignAdManagers", schema);
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.Countries)
                .WithMany(c => c.Campaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("CountryCode");
                    x.ToTable("CampaignCountries", schema);
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.TrafficTypes)
                .WithMany(c => c.Campaigns)
                .Map(c =>
                {
                    c.MapLeftKey("Pid");
                    c.MapRightKey("TrafficTypeId");
                    c.ToTable("CampaignTrafficTypes", schema);
                });

            modelBuilder.Entity<Campaign>().ToTable("Campaigns", schema);
            modelBuilder.Entity<Conversion>().ToTable("Conversions", schema);
            modelBuilder.Entity<Country>().ToTable("Countries", schema);
            modelBuilder.Entity<DailySummary>().ToTable("DailySummaries", schema);
            modelBuilder.Entity<Person>().ToTable("People", schema);
            modelBuilder.Entity<Status>().ToTable("Status", schema);
            modelBuilder.Entity<TrafficType>().ToTable("TrafficTypes", schema);
            modelBuilder.Entity<Vertical>().ToTable("Verticals", schema);
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Vertical> Verticals { get; set; }
        public DbSet<TrafficType> TrafficTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Conversion> Conversions { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
    }
}
