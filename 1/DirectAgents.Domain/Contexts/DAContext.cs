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
            modelBuilder.Entity<Offer>().ToTable("Offer", cakeSchema);

            modelBuilder.Entity<OfferDailySummary>()
                .HasKey(t => new { t.OfferId, t.Date })
                .ToTable("OfferDailySummary", cakeSchema);
        }

        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferInfo> OfferInfos { get; set; }
        public DbSet<OfferDailySummary> OfferDailySummaries { get; set; }
    }
}
