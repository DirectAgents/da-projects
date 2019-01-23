using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;

namespace DirectAgents.Domain.Contexts
{
    public class ClientPortalProgContext : DbContext
    {
        const string adrollSchema = "adr";
        const string dbmSchema = "dbm";
        const string tdSchema = "td";

        public ClientPortalProgContext()
        {
            var adapter = (IObjectContextAdapter)this;
            var objectContext = adapter.ObjectContext;
            objectContext.CommandTimeout = 3 * 60; // value in seconds
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.HasDefaultSchema(tdSchema); //can't do this b/c __MigrationHistory table is under dbo schema

            modelBuilder.Entity<Employee>().ToTable("Employee", "dbo");

            // TD
            modelBuilder.Entity<Advertiser>().ToTable("Advertiser", tdSchema);
            modelBuilder.Entity<Campaign>().ToTable("Campaign", tdSchema);
            modelBuilder.Entity<BudgetInfo>().ToTable("BudgetInfo", tdSchema);
            modelBuilder.Entity<PlatformBudgetInfo>().ToTable("PlatformBudgetInfo", tdSchema);
            modelBuilder.Entity<PlatColMapping>().ToTable("PlatColMapping", tdSchema);
            modelBuilder.Entity<Platform>().ToTable("Platform", tdSchema);
            modelBuilder.Entity<ExtAccount>().ToTable("Account", tdSchema);
            modelBuilder.Entity<Network>().ToTable("Network", tdSchema);
            modelBuilder.Entity<DailySummary>().ToTable("DailySummary", tdSchema);
            modelBuilder.Entity<Strategy>().ToTable("Strategy", tdSchema);
            modelBuilder.Entity<StrategySummary>().ToTable("StrategySummary", tdSchema);
            modelBuilder.Entity<AdSet>().ToTable("AdSet", tdSchema);
            modelBuilder.Entity<AdSetSummary>().ToTable("AdSetSummary", tdSchema);
            modelBuilder.Entity<TDad>().ToTable("Ad", tdSchema);
            modelBuilder.Entity<TDadExternalId>().ToTable("AdExternalId", tdSchema);
            modelBuilder.Entity<TDadSummary>().ToTable("AdSummary", tdSchema);
            modelBuilder.Entity<Site>().ToTable("Site", tdSchema);
            modelBuilder.Entity<SiteSummary>().ToTable("SiteSummary", tdSchema);
            modelBuilder.Entity<ExtraItem>().ToTable("ExtraItem", tdSchema);
            modelBuilder.Entity<Conv>().ToTable("Conv", tdSchema);
            modelBuilder.Entity<ConvCity>().ToTable("ConvCity", tdSchema);
            modelBuilder.Entity<ConvCountry>().ToTable("ConvCountry", tdSchema);
            modelBuilder.Entity<ActionType>().ToTable("ActionType", tdSchema);
            modelBuilder.Entity<StrategyAction>().ToTable("StrategyAction", tdSchema);
            modelBuilder.Entity<AdSetAction>().ToTable("AdSetAction", tdSchema);
            modelBuilder.Entity<EntityType>().ToTable("Type", tdSchema);
            modelBuilder.Entity<Keyword>().ToTable("Keyword", tdSchema);
            modelBuilder.Entity<KeywordSummary>().ToTable("KeywordSummary", tdSchema);
            modelBuilder.Entity<SearchTerm>().ToTable("SearchTerm", tdSchema);
            modelBuilder.Entity<SearchTermSummary>().ToTable("SearchTermSummary", tdSchema);
            modelBuilder.Entity<MetricType>().ToTable("MetricType", tdSchema);
            modelBuilder.Entity<DailySummaryMetric>().ToTable("DailySummaryMetric", tdSchema);
            modelBuilder.Entity<StrategySummaryMetric>().ToTable("StrategySummaryMetric", tdSchema);
            modelBuilder.Entity<AdSetSummaryMetric>().ToTable("AdSetSummaryMetric", tdSchema);
            modelBuilder.Entity<TDadSummaryMetric>().ToTable("AdSummaryMetric", tdSchema);
            modelBuilder.Entity<KeywordSummaryMetric>().ToTable("KeywordSummaryMetric", tdSchema);
            modelBuilder.Entity<SearchTermSummaryMetric>().ToTable("SearchTermSummaryMetric", tdSchema);

            //TD Vendor
            modelBuilder.Entity<VendorProduct>().ToTable("VProduct", tdSchema);
            modelBuilder.Entity<VendorCategory>().ToTable("VCategory", tdSchema);
            modelBuilder.Entity<VendorSubcategory>().ToTable("VSubcategory", tdSchema);
            modelBuilder.Entity<VendorBrand>().ToTable("VBrand", tdSchema);
            modelBuilder.Entity<VendorParentProduct>().ToTable("VParentProduct", tdSchema);
            modelBuilder.Entity<VendorProductSummaryMetric>().ToTable("VProductSummaryMetric", tdSchema);
            modelBuilder.Entity<VendorCategorySummaryMetric>().ToTable("VCategorySummaryMetric", tdSchema);
            modelBuilder.Entity<VendorSubcategorySummaryMetric>().ToTable("VSubcategorySummaryMetric", tdSchema);
            modelBuilder.Entity<VendorBrandSummaryMetric>().ToTable("VBrandSummaryMetric", tdSchema);
            modelBuilder.Entity<VendorParentProductSummaryMetric>().ToTable("VParentProductSummaryMetric", tdSchema);

            modelBuilder.Entity<Campaign>().Property(c => c.BaseFee).HasPrecision(14, 2);
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudgetInfo.MediaSpend).HasPrecision(14, 2).HasColumnName("MediaSpend");
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudgetInfo.MgmtFeePct).HasPrecision(10, 5).HasColumnName("MgmtFeePct");
            modelBuilder.Entity<Campaign>().Property(c => c.DefaultBudgetInfo.MarginPct).HasPrecision(10, 5).HasColumnName("MarginPct");
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MediaSpend).HasPrecision(14, 2);
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MgmtFeePct).HasPrecision(10, 5);
            modelBuilder.Entity<BudgetInfo>().Property(b => b.MarginPct).HasPrecision(10, 5);
            modelBuilder.Entity<BudgetInfo>()
                .HasKey(b => new { b.CampaignId, b.Date });
            modelBuilder.Entity<PlatformBudgetInfo>().Property(b => b.MediaSpend).HasPrecision(14, 2);
            modelBuilder.Entity<PlatformBudgetInfo>().Property(b => b.MgmtFeePct).HasPrecision(10, 5);
            modelBuilder.Entity<PlatformBudgetInfo>().Property(b => b.MarginPct).HasPrecision(10, 5);
            modelBuilder.Entity<PlatformBudgetInfo>()
                .HasKey(b => new { b.CampaignId, b.PlatformId, b.Date });
            modelBuilder.Entity<PlatColMapping>().HasRequired(p => p.Platform);
            modelBuilder.Entity<DailySummary>()
                .HasKey(ds => new { ds.Date, ds.AccountId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<DailySummary>().Property(ds => ds.PostClickRev).HasPrecision(18, 4);
            modelBuilder.Entity<DailySummary>().Property(ds => ds.PostViewRev).HasPrecision(18, 4);
            modelBuilder.Entity<StrategySummary>()
                .HasKey(ss => new { ss.Date, ss.StrategyId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<StrategySummary>().Property(ds => ds.PostClickRev).HasPrecision(18, 4);
            modelBuilder.Entity<StrategySummary>().Property(ds => ds.PostViewRev).HasPrecision(18, 4);
            modelBuilder.Entity<AdSetSummary>()
                .HasKey(x => new { x.Date, x.AdSetId })
                .Property(x => x.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<AdSetSummary>().Property(ds => ds.PostClickRev).HasPrecision(18, 4);
            modelBuilder.Entity<AdSetSummary>().Property(ds => ds.PostViewRev).HasPrecision(18, 4);
            modelBuilder.Entity<TDadSummary>()
                .HasKey(s => new { s.Date, s.TDadId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<KeywordSummary>()
                .HasKey(s => new { s.Date, s.KeywordId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<SearchTermSummary>()
                .HasKey(s => new { s.Date, s.SearchTermId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<SiteSummary>()
                .HasKey(s => new { s.Date, s.SiteId, s.AccountId })
                .Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<ExtraItem>().Property(i => i.Cost).HasPrecision(14, 2);
            modelBuilder.Entity<ExtraItem>().Property(i => i.Revenue).HasPrecision(14, 2);
            modelBuilder.Entity<Conv>().Property(c => c.ConvVal).HasPrecision(18, 6);
            modelBuilder.Entity<StrategyAction>()
                .HasKey(x => new { x.Date, x.StrategyId, x.ActionTypeId });
            modelBuilder.Entity<AdSetAction>()
                .HasKey(x => new { x.Date, x.AdSetId, x.ActionTypeId });
            modelBuilder.Entity<AdSetAction>().Property(x => x.PostClickVal).HasPrecision(18, 4);
            modelBuilder.Entity<AdSetAction>().Property(x => x.PostViewVal).HasPrecision(18, 4);
            modelBuilder.Entity<TDadExternalId>().HasKey(x => new { x.AdId, x.TypeId });
            SetupSummaryMetricModel<DailySummaryMetric>(modelBuilder, "AccountId");
            modelBuilder.Entity<DailySummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);
            SetupSummaryMetricModel<StrategySummaryMetric>(modelBuilder, "StrategyId");
            modelBuilder.Entity<StrategySummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);
            SetupSummaryMetricModel<AdSetSummaryMetric>(modelBuilder, "AdSetId");
            modelBuilder.Entity<AdSetSummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);
            SetupSummaryMetricModel<TDadSummaryMetric>(modelBuilder, "TDadId");
            modelBuilder.Entity<TDadSummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);
            SetupSummaryMetricModel<KeywordSummaryMetric>(modelBuilder, "KeywordId");
            modelBuilder.Entity<KeywordSummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);
            SetupSummaryMetricModel<SearchTermSummaryMetric>(modelBuilder, "SearchTermId");
            modelBuilder.Entity<SearchTermSummary>()
                .HasMany(x => x.Metrics)
                .WithRequired()
                .HasForeignKey(x => new { x.Date, x.EntityId })
                .WillCascadeOnDelete(false);

            //TD Vendor
            SetupSummaryMetricModel<VendorProductSummaryMetric>(modelBuilder, "ProductId");
            SetupSummaryMetricModel<VendorSubcategorySummaryMetric>(modelBuilder, "SubcategoryId");
            SetupSummaryMetricModel<VendorCategorySummaryMetric>(modelBuilder, "CategoryId");
            SetupSummaryMetricModel<VendorBrandSummaryMetric>(modelBuilder, "BrandId");
            SetupSummaryMetricModel<VendorParentProductSummaryMetric>(modelBuilder, "ParentProductId");

            // AdRoll
            modelBuilder.Entity<Advertisable>().ToTable("Advertisable", adrollSchema);
            modelBuilder.Entity<Ad>().ToTable("Ad", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .HasKey(ds => new { ds.Date, ds.AdId })
                .ToTable("AdDailySummary", adrollSchema);
            modelBuilder.Entity<AdDailySummary>()
                .Property(ds => ds.Cost).HasPrecision(18, 6);

            // DBM
            modelBuilder.Entity<InsertionOrder>().ToTable("InsertionOrder", dbmSchema);
            modelBuilder.Entity<Creative>().ToTable("Creative", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .HasKey(cds => new { cds.Date, cds.InsertionOrderID, cds.CreativeID })
                .ToTable("CreativeDailySummary", dbmSchema);
            modelBuilder.Entity<CreativeDailySummary>()
                .Property(cds => cds.Revenue).HasPrecision(18, 6);

            //TD Vendor

        }

        public DbSet<Employee> Employees { get; set; }

        // TD
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<BudgetInfo> BudgetInfos { get; set; }
        public DbSet<PlatformBudgetInfo> PlatformBudgetInfos { get; set; }
        public DbSet<PlatColMapping> PlatColMappings { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<ExtAccount> ExtAccounts { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<DailySummary> DailySummaries { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<StrategySummary> StrategySummaries { get; set; }
        public DbSet<AdSet> AdSets { get; set; }
        public DbSet<AdSetSummary> AdSetSummaries { get; set; }
        public DbSet<TDad> TDads { get; set; }
        public DbSet<TDadExternalId> TDadExternalIds { get; set; }
        public DbSet<TDadSummary> TDadSummaries { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteSummary> SiteSummaries { get; set; }
        public DbSet<ExtraItem> ExtraItems { get; set; }
        public DbSet<Conv> Convs { get; set; }
        public DbSet<ConvCity> ConvCities { get; set; }
        public DbSet<ConvCountry> ConvCountries { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<StrategyAction> StrategyActions { get; set; }
        public DbSet<AdSetAction> AdSetActions { get; set; }
        public DbSet<EntityType> Types { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<KeywordSummary> KeywordSummaries { get; set; }
        public DbSet<SearchTerm> SearchTerms { get; set; }
        public DbSet<SearchTermSummary> SearchTermSummaries { get; set; }
        public DbSet<MetricType> MetricTypes { get; set; }
        public DbSet<DailySummaryMetric> DailySummaryMetrics { get; set; }
        public DbSet<StrategySummaryMetric> StrategySummaryMetrics { get; set; }
        public DbSet<AdSetSummaryMetric> AdSetSummaryMetrics { get; set; }
        public DbSet<TDadSummaryMetric> TDadSummaryMetrics { get; set; }
        public DbSet<KeywordSummaryMetric> KeywordSummaryMetrics { get; set; }
        public DbSet<SearchTermSummaryMetric> SearchTermSummaryMetrics { get; set; }

        //TD Vendor
        public DbSet<VendorProduct> VendorProducts { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<VendorSubcategory> VendorSubcategories { get; set; }
        public DbSet<VendorBrand> VendorBrands { get; set; }
        public DbSet<VendorParentProduct> VendorParentProducts { get; set; }
        public DbSet<VendorProductSummaryMetric> VendorProductSummaryMetrics { get; set; }
        public DbSet<VendorCategorySummaryMetric> VendorCategorySummaryMetrics { get; set; }
        public DbSet<VendorSubcategorySummaryMetric> VendorSubcategorySummaryMetrics { get; set; }
        public DbSet<VendorBrandSummaryMetric> VendorBrandSummaryMetrics { get; set; }
        public DbSet<VendorParentProductSummaryMetric> VendorParentProductSummaryMetrics { get; set; }

        // AdRoll
        public DbSet<Advertisable> Advertisables { get; set; }
        public DbSet<Ad> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdRollAdDailySummaries { get; set; }

        // DBM
        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<CreativeDailySummary> DBMCreativeDailySummaries { get; set; }

        private void SetupSummaryMetricModel<TSummaryMetric>(DbModelBuilder modelBuilder, string entityColumnName)
            where TSummaryMetric : SummaryMetric
        {
            modelBuilder.Entity<TSummaryMetric>().HasKey(s => new {s.Date, s.EntityId, s.MetricTypeId});
            modelBuilder.Entity<TSummaryMetric>().Property(x => x.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TSummaryMetric>().Property(t => t.Value).HasPrecision(18, 6);
        }
    }
}
