using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Domain.Entities.Screen;

namespace DirectAgents.Domain.Contexts
{
    public class DAContext : DbContext
    {
        //public DAContext() : base() { }
        const string cakeSchema = "cake";
        const string adrollSchema = "adr";
        const string screenSchema = "screen";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Cake
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

            // AdRoll
            modelBuilder.Entity<Advertisable>().ToTable("Advertisable", adrollSchema);
            modelBuilder.Entity<AdvertisableStat>()
                .HasKey(t => new { t.Date, t.AdvertisableId })
                .ToTable("AdvertisableStat", adrollSchema);
            modelBuilder.Entity<AdvertisableStat>()
                .Property(t => t.Cost).HasPrecision(18, 6);

            // Screen
            modelBuilder.Entity<Salesperson>().ToTable("Salesperson", screenSchema);
            modelBuilder.Entity<SalespersonStat>()
                .HasKey(t => new { t.Date, t.SalespersonId })
                .ToTable("SalespersonStat", screenSchema);

            // general
            modelBuilder.Entity<Variable>()
                .Property(t => t.DecVal).HasPrecision(18, 6);
        }

        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Vertical> Verticals { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<OfferStatus> OfferStatuses { get; set; }
        public DbSet<OfferDailySummary> OfferDailySummaries { get; set; }

        public DbSet<Advertisable> Advertisables { get; set; }
        public DbSet<AdvertisableStat> AdvertisableStats { get; set; }

        public DbSet<Salesperson> Salespeople { get; set; }
        public DbSet<SalespersonStat> SalespersonStats { get; set; }

        public DbSet<OfferBudget> OfferBudgets { get; set; }
        public DbSet<Variable> Variables { get; set; }
    }
}
