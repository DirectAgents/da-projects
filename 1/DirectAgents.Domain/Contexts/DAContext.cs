using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DirectAgents.Domain.Contexts
{
    public class DAContext : DbContext
    {
        //public DAContext() : base() { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            string cakeSchema = "cake";
            modelBuilder.Entity<Advertiser>().ToTable("Advertiser", cakeSchema);
            modelBuilder.Entity<Contact>().ToTable("Contact", cakeSchema);
            modelBuilder.Entity<Role>().ToTable("Role", cakeSchema);
            modelBuilder.Entity<Offer>().ToTable("Offer", cakeSchema);
            modelBuilder.Entity<Vertical>().ToTable("Vertical", cakeSchema);
            modelBuilder.Entity<OfferType>().ToTable("OfferType", cakeSchema);
            modelBuilder.Entity<OfferStatus>().ToTable("OfferStatus", cakeSchema);

            modelBuilder.Entity<OfferDailySummary>()
                .HasKey(t => new { t.OfferId, t.Date })
                .ToTable("OfferDailySummary", cakeSchema);
        }

        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Vertical> Verticals { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<OfferStatus> OfferStatuses { get; set; }

        public DbSet<OfferDailySummary> OfferDailySummaries { get; set; }
        public DbSet<OfferBudget> OfferBudgets { get; set; }
    }
}
