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
                    x.MapLeftKey("PersonId");
                    x.MapRightKey("Pid");
                    x.ToTable("CampaignAccountManagers");
                });

            modelBuilder.Entity<Campaign>()
                .HasMany(u => u.MediaBuyers)
                .WithMany(t => t.MediaBuyerCampaigns)
                .Map(x =>
                {
                    x.MapLeftKey("PersonId");
                    x.MapRightKey("Pid");
                    x.ToTable("CampaignMediaBuyers");
                });
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Person> People { get; set; }
    }
}
