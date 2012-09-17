using System.Data.Entity;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campaign>()
                .HasMany(u => u.AccountManagers)
                .WithMany(t => t.AccountManagerCampaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("PersonId");
                    x.ToTable("CampaignAccountManagers");
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(u => u.AdManagers)
                .WithMany(t => t.AdManagerCampaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("PersonId");
                    x.ToTable("CampaignAdManagers");
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(c => c.Countries)
                .WithMany(c => c.Campaigns)
                .Map(x =>
                {
                    x.MapLeftKey("Pid");
                    x.MapRightKey("CountryCode");
                    x.ToTable("CampaignCountries");
                });
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Vertical> Verticals { get; set; }
    }
}
