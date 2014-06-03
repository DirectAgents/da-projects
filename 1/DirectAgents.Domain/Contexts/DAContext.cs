using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;
using System.Data.Entity;

namespace DirectAgents.Domain.Contexts
{
    public class DAContext : DbContext
    {
        //public DAContext() : base() { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string cakeSchema = "cake";
            modelBuilder.Entity<Advertiser>().ToTable("Advertiser", cakeSchema);
            modelBuilder.Entity<Contact>().ToTable("Contact", cakeSchema);
            modelBuilder.Entity<Role>().ToTable("Role", cakeSchema);

            modelBuilder.Entity<Offer>()
                .Map(m =>
                {
                    m.Properties(o => new { o.OfferId, o.AdvertiserId, o.OfferName, o.DefaultPriceFormatName, o.CurrencyAbbr, o.DateCreated });
                    m.ToTable("Offer", cakeSchema);
                })
                .Map(m =>
                {
                    m.Properties(o => new { o.Budget, o.BudgetIsMonthly, o.BudgetStart });
                    m.ToTable("OfferInfo");
                });

            modelBuilder.Entity<OfferDailySummary>()
                .HasKey(t => new { t.OfferId, t.Date })
                .ToTable("OfferDailySummary", cakeSchema);
        }

        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferDailySummary> OfferDailySummaries { get; set; }
    }
}
