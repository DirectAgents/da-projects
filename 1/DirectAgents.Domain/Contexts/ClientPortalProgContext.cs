using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Adform;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;
using DirectAgents.Domain.Entities.CPProg.AmazonAttribution;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using DirectAgents.Domain.Entities.CPProg.CJ;
using DirectAgents.Domain.Entities.CPProg.DBM.Entities;
using DirectAgents.Domain.Entities.CPProg.DBM.SummaryMetrics;
using DirectAgents.Domain.Entities.CPProg.Kochava;
using DirectAgents.Domain.Entities.CPProg.Facebook.Ad;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using DirectAgents.Domain.Entities.CPProg.Roku;
using DirectAgents.Domain.Entities.CPProg.YAM;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;
using DirectAgents.Domain.Entities.CPProg.Vendor.RepeatPurchaseBehavior;

namespace DirectAgents.Domain.Contexts
{
    public class ClientPortalProgContext : DbContext
    {
        const string adrollSchema = "adr";
        const string dbmSchema = "dbm";
        const string tdSchema = "td";
        const string admSchema = "adm";

        public ClientPortalProgContext()
        {
            var adapter = (IObjectContextAdapter)this;
            var objectContext = adapter.ObjectContext;
            objectContext.CommandTimeout = 5 * 60; // value in seconds
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.HasDefaultSchema(tdSchema); //can't do this b/c __MigrationHistory table is under dbo schema

            modelBuilder.Entity<Employee>().ToTable("Employee", "dbo");

            // Adm
            modelBuilder.Entity<JobRequestExecution>().ToTable("JobRequestExecution", admSchema);
            modelBuilder.Entity<JobRequest>().ToTable("JobRequest", admSchema);

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

            //TD Amazon Attribution
            modelBuilder.Entity<AttributionSummary>().ToTable("AttributionSummary", tdSchema);

            //TD Vendor
            modelBuilder.Entity<VendorProduct>().ToTable("VProduct", tdSchema);
            modelBuilder.Entity<VendorCategory>().ToTable("VCategory", tdSchema);
            modelBuilder.Entity<VendorSubcategory>().ToTable("VSubcategory", tdSchema);
            modelBuilder.Entity<VendorBrand>().ToTable("VBrand", tdSchema);
            modelBuilder.Entity<VendorGeographicSalesInsightsProduct>().ToTable("VGeographicSalesInsightsProduct", tdSchema);
            modelBuilder.Entity<VendorNetPpmWeeklyProduct>().ToTable("VNetPpmWeeklyProduct", tdSchema);
            modelBuilder.Entity<VendorNetPpmMonthlyProduct>().ToTable("VNetPpmMonthlyProduct", tdSchema);
            modelBuilder.Entity<VendorNetPpmYearlyProduct>().ToTable("VNetPpmYearlyProduct", tdSchema);
            modelBuilder.Entity<VendorRepeatPurchaseBehaviorMonthlyProduct>().ToTable("VRepeatPurchaseBehaviorMonthlyProduct", tdSchema);
            modelBuilder.Entity<VendorRepeatPurchaseBehaviorQuaterlyProduct>().ToTable("VRepeatPurchaseBehaviorQuaterlyProduct", tdSchema);
            modelBuilder.Entity<VendorParentProduct>().ToTable("VParentProduct", tdSchema);
            modelBuilder.Entity<VendorProductSummaryMetric>().ToTable("VProductSummaryMetric", tdSchema);
            modelBuilder.Entity<VendorCategorySummaryMetric>().ToTable("VCategorySummaryMetric", tdSchema);
            modelBuilder.Entity<VendorSubcategorySummaryMetric>().ToTable("VSubcategorySummaryMetric", tdSchema);
            modelBuilder.Entity<VendorBrandSummaryMetric>().ToTable("VBrandSummaryMetric", tdSchema);
            modelBuilder.Entity<VendorParentProductSummaryMetric>().ToTable("VParentProductSummaryMetric", tdSchema);
            modelBuilder.Entity<VcdAnalyticItem>().ToTable("VcdAnalytic", tdSchema);

            //TD Facebook
            modelBuilder.Entity<FbAd>().ToTable("FbAd", tdSchema);
            modelBuilder.Entity<FbAdAction>().ToTable("FbAdAction", tdSchema);
            modelBuilder.Entity<FbAdSummary>().ToTable("FbAdSummary", tdSchema);
            modelBuilder.Entity<FbCreative>().ToTable("FbCreative", tdSchema);
            modelBuilder.Entity<FbAdSet>().ToTable("FbAdSet", tdSchema);
            modelBuilder.Entity<FbAdSetAction>().ToTable("FbAdSetAction", tdSchema);
            modelBuilder.Entity<FbAdSetSummary>().ToTable("FbAdSetSummary", tdSchema);
            modelBuilder.Entity<FbCampaign>().ToTable("FbCampaign", tdSchema);
            modelBuilder.Entity<FbCampaignAction>().ToTable("FbCampaignAction", tdSchema);
            modelBuilder.Entity<FbCampaignSummary>().ToTable("FbCampaignSummary", tdSchema);
            modelBuilder.Entity<FbDailySummary>().ToTable("FbDailySummary", tdSchema);
            modelBuilder.Entity<FbActionType>().ToTable("FbActionType", tdSchema);
            modelBuilder.Entity<FbReachMetric>().ToTable("FbReachMetric", tdSchema);
            modelBuilder.Entity<FbCampaignReachMetric>().ToTable("FbCampaignReachMetric", tdSchema);

            //TD Adform
            modelBuilder.Entity<AdfDailySummary>().ToTable("AdfDailySummary", tdSchema);
            modelBuilder.Entity<AdfCampaignSummary>().ToTable("AdfCampaignSummary", tdSchema);
            modelBuilder.Entity<AdfLineItemSummary>().ToTable("AdfLineItemSummary", tdSchema);
            modelBuilder.Entity<AdfTrackingPointSummary>().ToTable("AdfTrackingPointSummary", tdSchema);
            modelBuilder.Entity<AdfBannerSummary>().ToTable("AdfBannerSummary", tdSchema);
            modelBuilder.Entity<AdfCampaign>().ToTable("AdfCampaign", tdSchema);
            modelBuilder.Entity<AdfLineItem>().ToTable("AdfLineItem", tdSchema);
            modelBuilder.Entity<AdfTrackingPoint>().ToTable("AdfTrackingPoint", tdSchema);
            modelBuilder.Entity<AdfBanner>().ToTable("AdfBanner", tdSchema);
            modelBuilder.Entity<AdfMediaType>().ToTable("AdfMediaType", tdSchema);

            //TD CJ
            modelBuilder.Entity<CjAdvertiserCommission>().ToTable("CjAdvertiserCommission", tdSchema);
            modelBuilder.Entity<CjAdvertiserCommissionItem>().ToTable("CjAdvertiserCommissionItem", tdSchema);

            //TD DSP
            modelBuilder.Entity<DspAdvertiser>().ToTable("DspAdvertiser", tdSchema);
            modelBuilder.Entity<DspOrder>().ToTable("DspOrder", tdSchema);
            modelBuilder.Entity<DspLineItem>().ToTable("DspLineItem", tdSchema);
            modelBuilder.Entity<DspCreative>().ToTable("DspCreative", tdSchema);
            modelBuilder.Entity<DspAdvertiserDailyMetricValues>().ToTable("DspAdvertiserDailyMetricValues", tdSchema);
            modelBuilder.Entity<DspOrderMetricValues>().ToTable("DspOrderMetricValues", tdSchema);
            modelBuilder.Entity<DspLineDailyMetricValues>().ToTable("DspLineDailyMetricValues", tdSchema);
            modelBuilder.Entity<DspCreativeDailyMetricValues>().ToTable("DspCreativeDailyMetricValues", tdSchema);

            //TD Kochava
            modelBuilder.Entity<KochavaItem>().ToTable("KochavaItem", tdSchema);

            // YAM
            modelBuilder.Entity<YamCampaign>().ToTable("YamCampaign", tdSchema);
            modelBuilder.Entity<YamLine>().ToTable("YamLine", tdSchema);
            modelBuilder.Entity<YamAd>().ToTable("YamAd", tdSchema);
            modelBuilder.Entity<YamCreative>().ToTable("YamCreative", tdSchema);
            modelBuilder.Entity<YamPixel>().ToTable("YamPixel", tdSchema);
            modelBuilder.Entity<YamDailySummary>().ToTable("YamDailySummary", tdSchema);
            modelBuilder.Entity<YamCampaignSummary>().ToTable("YamCampaignSummary", tdSchema);
            modelBuilder.Entity<YamLineSummary>().ToTable("YamLineSummary", tdSchema);
            modelBuilder.Entity<YamAdSummary>().ToTable("YamAdSummary", tdSchema);
            modelBuilder.Entity<YamCreativeSummary>().ToTable("YamCreativeSummary", tdSchema);
            modelBuilder.Entity<YamPixelSummary>().ToTable("YamPixelSummary", tdSchema);

            // Roku
            modelBuilder.Entity<RokuSummary>().ToTable("RokuSummary", tdSchema);

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

            //TD Facebook
            SetupFacebookModel(modelBuilder);

            //TD Adform
            SetupAdformModel(modelBuilder);

            //TD Vendor
            SetupSummaryMetricModel<VendorProductSummaryMetric>(modelBuilder, "ProductId");
            SetupSummaryMetricModel<VendorSubcategorySummaryMetric>(modelBuilder, "SubcategoryId");
            SetupSummaryMetricModel<VendorCategorySummaryMetric>(modelBuilder, "CategoryId");
            SetupSummaryMetricModel<VendorBrandSummaryMetric>(modelBuilder, "BrandId");
            SetupSummaryMetricModel<VendorParentProductSummaryMetric>(modelBuilder, "ParentProductId");
            SetupVcdAnalyticModelValues(modelBuilder);

            //TD DSP
            SetupDailyMetricModelValues<DspAdvertiserDailyMetricValues>(modelBuilder, "AdvertiserId");
            SetupDailyMetricModelValues<DspOrderMetricValues>(modelBuilder, "OrderId");
            SetupDailyMetricModelValues<DspLineDailyMetricValues>(modelBuilder, "LineItemId");
            SetupDailyMetricModelValues<DspCreativeDailyMetricValues>(modelBuilder, "CreativeId");

            //TD CJ
            SetupCjModelValues(modelBuilder);

            // YAM
            SetupYamDailyMetricModelValues<YamDailySummary>(modelBuilder, "AccountId");
            SetupYamDailyMetricModelValues<YamCampaignSummary>(modelBuilder, "CampaignId");
            SetupYamDailyMetricModelValues<YamLineSummary>(modelBuilder, "LineId");
            SetupYamDailyMetricModelValues<YamAdSummary>(modelBuilder, "AdId");
            SetupYamDailyMetricModelValues<YamCreativeSummary>(modelBuilder, "CreativeId");
            SetupYamDailyMetricModelValues<YamPixelSummary>(modelBuilder, "PixelId");
            modelBuilder.Entity<YamLine>()
                .HasMany(g => g.Ads)
                .WithRequired(s => s.Line)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<YamCreative>()
                .HasMany(g => g.Ads)
                .WithRequired(s => s.Creative)
                .WillCascadeOnDelete(false);

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

            // TD DBM
            modelBuilder.Entity<DbmAdvertiser>().ToTable("DbmAdvertiser", tdSchema);
            modelBuilder.Entity<DbmCampaign>().ToTable("DbmCampaign", tdSchema);
            modelBuilder.Entity<DbmInsertionOrder>().ToTable("DbmInsertionOrder", tdSchema);
            modelBuilder.Entity<DbmLineItem>().ToTable("DbmLineItem", tdSchema);
            modelBuilder.Entity<DbmCreative>().ToTable("DbmCreative", tdSchema);
            modelBuilder.Entity<DbmLineItemSummary>().ToTable("DbmLineItemSummary", tdSchema);
            modelBuilder.Entity<DbmCreativeSummary>().ToTable("DbmCreativeSummary", tdSchema);

            SetupDbmBasicSummaryMetricModelValues<DbmLineItemSummary>(modelBuilder, "LineItemId");
            SetupDbmBasicSummaryMetricModelValues<DbmCreativeSummary>(modelBuilder, "CreativeId");
        }

        public DbSet<Employee> Employees { get; set; }

        // Adm
        public DbSet<JobRequestExecution> JobRequestExecutions { get; set; }
        public DbSet<JobRequest> JobExecutionRequests { get; set; }

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

        //TD Amazon Attribution
        public DbSet<AttributionSummary> AmazonAttributionSummaries { get; set; }

        //TD Vendor
        public DbSet<VendorProduct> VendorProducts { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<VendorSubcategory> VendorSubcategories { get; set; }
        public DbSet<VendorBrand> VendorBrands { get; set; }
        public DbSet<VendorParentProduct> VendorParentProducts { get; set; }
        public DbSet<VendorGeographicSalesInsightsProduct> VendorGeographicSalesInsightsProducts { get; set; }
        public DbSet<VendorNetPpmWeeklyProduct> VendorNetPpmWeeklyProducts { get; set; }
        public DbSet<VendorNetPpmMonthlyProduct> VendorNetPpmMonthlyProducts { get; set; }
        public DbSet<VendorNetPpmYearlyProduct> VendorNetPpmYearlyProducts { get; set; }
        public DbSet<VendorRepeatPurchaseBehaviorMonthlyProduct> VendorRepeatPurchaseBehaviorMonthlyProducts { get; set; }
        public DbSet<VendorRepeatPurchaseBehaviorQuaterlyProduct> VendorRepeatPurchaseBehaviorQuaterlyProducts { get; set; }
        public DbSet<VendorProductSummaryMetric> VendorProductSummaryMetrics { get; set; }
        public DbSet<VendorCategorySummaryMetric> VendorCategorySummaryMetrics { get; set; }
        public DbSet<VendorSubcategorySummaryMetric> VendorSubcategorySummaryMetrics { get; set; }
        public DbSet<VendorBrandSummaryMetric> VendorBrandSummaryMetrics { get; set; }
        public DbSet<VendorParentProductSummaryMetric> VendorParentProductSummaryMetrics { get; set; }
        public DbSet<VcdAnalyticItem> VcdAnalytic { get; set; }

        //TD Facebook
        public DbSet<FbAd> FbAds { get; set; }
        public DbSet<FbAdAction> FbAdActions { get; set; }
        public DbSet<FbAdSummary> FbAdSummaries { get; set; }
        public DbSet<FbCreative> FbCreatives { get; set; }
        public DbSet<FbAdSet> FbAdSets { get; set; }
        public DbSet<FbAdSetAction> FbAdSetActions { get; set; }
        public DbSet<FbAdSetSummary> FbAdSetSummaries { get; set; }
        public DbSet<FbCampaign> FbCampaigns { get; set; }
        public DbSet<FbCampaignAction> FbCampaignActions { get; set; }
        public DbSet<FbCampaignSummary> FbCampaignSummaries { get; set; }
        public DbSet<FbDailySummary> FbDailySummaries { get; set; }
        public DbSet<FbActionType> FbActionTypes { get; set; }

        //TD Adform
        public DbSet<AdfDailySummary> AdfDailySummaries { get; set; }
        public DbSet<AdfCampaignSummary> AdfCampaignSummaries { get; set; }
        public DbSet<AdfLineItemSummary> AdfLineItemSummaries { get; set; }
        public DbSet<AdfTrackingPointSummary> AdfTrackingPointSummaries { get; set; }
        public DbSet<AdfBannerSummary> AdfBannerSummaries { get; set; }
        public DbSet<AdfCampaign> AdfCampaigns { get; set; }
        public DbSet<AdfLineItem> AdfLineItems { get; set; }
        public DbSet<AdfTrackingPoint> AdfTrackingPoints { get; set; }
        public DbSet<AdfBanner> AdfBanners { get; set; }
        public DbSet<AdfMediaType> AdfMediaTypes { get; set; }

        //TD DSP
        public DbSet<DspAdvertiser> DspAdvertisers { get; set; }
        public DbSet<DspOrder> DspOrders { get; set; }
        public DbSet<DspLineItem> DspLineItems { get; set; }
        public DbSet<DspCreative> DspCreatives { get; set; }
        public DbSet<DspAdvertiserDailyMetricValues> DspAdvertisersMetricValues { get; set; }
        public DbSet<DspOrderMetricValues> DspOrdersMetricValues { get; set; }
        public DbSet<DspLineDailyMetricValues> DspLineItemsMetricValues { get; set; }
        public DbSet<DspCreativeDailyMetricValues> DspCreativesMetricValues { get; set; }

        //TD CJ
        public DbSet<CjAdvertiserCommission> CjAdvertiserCommissions { get; set; }
        public DbSet<CjAdvertiserCommissionItem> CjAdvertiserCommissionItems { get; set; }

        //TD Kochava
        public DbSet<KochavaItem> KochavaItems { get; set; }

        // YAM
        public DbSet<YamCampaign> YamCampaigns { get; set; }
        public DbSet<YamLine> YamLines { get; set; }
        public DbSet<YamAd> YamAds { get; set; }
        public DbSet<YamPixel> YamPixels { get; set; }
        public DbSet<YamCreative> YamCreatives { get; set; }
        public DbSet<YamDailySummary> YamDailySummaries { get; set; }
        public DbSet<YamCampaignSummary> YamCampaignSummaries { get; set; }
        public DbSet<YamLineSummary> YamLineSummaries { get; set; }
        public DbSet<YamAdSummary> YamAdSummaries { get; set; }
        public DbSet<YamPixelSummary> YamPixelSummaries { get; set; }
        public DbSet<YamCreativeSummary> YamCreativeSummaries { get; set; }

        // AdRoll
        public DbSet<Advertisable> Advertisables { get; set; }
        public DbSet<Ad> AdRollAds { get; set; }
        public DbSet<AdDailySummary> AdRollAdDailySummaries { get; set; }

        // DBM
        public DbSet<InsertionOrder> InsertionOrders { get; set; }
        public DbSet<Creative> Creatives { get; set; }
        public DbSet<CreativeDailySummary> DBMCreativeDailySummaries { get; set; }

        //TD DBM
        public DbSet<DbmCampaign> DbmCampaigns { get; set; }
        public DbSet<DbmInsertionOrder> DbmInsertionOrders { get; set; }
        public DbSet<DbmLineItem> DbmLineItems { get; set; }
        public DbSet<DbmLineItemSummary> DbmLineItemSummaries { get; set; }
        public DbSet<DbmCreative> DbmCreatives { get; set; }
        public DbSet<DbmCreativeSummary> DbmCreativeSummaries { get; set; }

        // Roku
        public DbSet<RokuSummary> RokuSummaries { get; set; }

        private void SetupCjModelValues(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CjAdvertiserCommission>().Property(t => t.AdvCommissionAmountUsd).HasPrecision(18, 6);
            modelBuilder.Entity<CjAdvertiserCommission>().Property(t => t.CjFeeUsd).HasPrecision(18, 6);
            modelBuilder.Entity<CjAdvertiserCommission>().Property(t => t.OrderDiscountUsd).HasPrecision(18, 6);
            modelBuilder.Entity<CjAdvertiserCommission>().Property(t => t.SaleAmountUsd).HasPrecision(18, 6);

            modelBuilder.Entity<CjAdvertiserCommissionItem>().Property(t => t.PerItemSaleAmountUsd).HasPrecision(18, 6);
            modelBuilder.Entity<CjAdvertiserCommissionItem>().Property(t => t.TotalCommissionUsd).HasPrecision(18, 6);

            modelBuilder.Entity<CjAdvertiserCommission>()
                .HasMany(g => g.Items)
                .WithRequired(s => s.Commission)
                .WillCascadeOnDelete();
        }

        private static void SetupDbmBasicSummaryMetricModelValues<TSummaryMetrics>(DbModelBuilder modelBuilder, string entityColumnName)
            where TSummaryMetrics : DbmBaseSummaryEntity
        {
            modelBuilder.Entity<TSummaryMetrics>().HasKey(m => m.Id);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.Revenue).HasPrecision(18, 6);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.Impressions);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.Clicks);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.PostClickConversions);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.PostViewConversions);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.CMPostClickRevenue).HasPrecision(18, 6);
            modelBuilder.Entity<TSummaryMetrics>().Property(m => m.CMPostViewRevenue).HasPrecision(18, 6);
        }

        private static void SetupVcdAnalyticModelValues(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.ShippedRevenue).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.ShippedUnits).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.OrderedUnits).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.ShippedCOGS).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.FreeReplacements).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.CustomerReturns).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.OrderedRevenue).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.LBB).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.RepOos).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.RepOosPercentOfTotal).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.RepOosPriorPeriodPercentChange).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.GlanceViews).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.SalesRank).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.AverageSalesPrice).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.SellableOnHandUnits).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.NumberOfCustomerReviews).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.NumberOfCustomerReviewsLifeToDate).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.AverageCustomerRating).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.FiveStars).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.FourStars).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.ThreeStars).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.TwoStars).HasPrecision(18, 6);
            modelBuilder.Entity<VcdAnalyticItem>().Property(t => t.OneStar).HasPrecision(18, 6);
        }

        private void SetupDailyMetricModelValues<TDailyMetricValues>(DbModelBuilder modelBuilder, string entityColumnName)
             where TDailyMetricValues : DspMetricValues
        {
            modelBuilder.Entity<TDailyMetricValues>().HasKey(s => new { s.Date, s.EntityId });
            modelBuilder.Entity<TDailyMetricValues>().Property(x => x.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.TotalCost).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.Impressions).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.ClickThroughs).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.TotalPixelEvents).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.TotalPixelEventsViews).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.TotalPixelEventsClicks).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.DPV).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.ATC).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.Purchase).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.PurchaseViews).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.PurchaseClicks).HasPrecision(18, 6);
        }

        private void SetupFacebookModel(DbModelBuilder modelBuilder)
        {
            SetupFacebookModelKeys(modelBuilder);
            SetupFacebookSummaryMetricModelValues<FbAdSummary>(modelBuilder);
            SetupFacebookSummaryMetricModelValues<FbAdSetSummary>(modelBuilder);
            SetupFacebookSummaryMetricModelValues<FbCampaignSummary>(modelBuilder);
            SetupFacebookSummaryMetricModelValues<FbDailySummary>(modelBuilder);
            SetupFacebookActionMetricModelValues<FbAdAction>(modelBuilder);
            SetupFacebookActionMetricModelValues<FbAdSetAction>(modelBuilder);
            SetupFacebookActionMetricModelValues<FbCampaignAction>(modelBuilder);
        }

        private void SetupFacebookModelKeys(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FbAdSummary>().HasKey(s => new { s.Date, s.AdId });
            modelBuilder.Entity<FbAdSetSummary>().HasKey(s => new { s.Date, s.AdSetId });
            modelBuilder.Entity<FbCampaignSummary>().HasKey(s => new { s.Date, s.CampaignId });
            modelBuilder.Entity<FbDailySummary>().HasKey(s => new { s.Date, s.AccountId });
            modelBuilder.Entity<FbAdAction>()
                .HasKey(x => new { x.Date, x.AdId, x.ActionTypeId });
            modelBuilder.Entity<FbAdSetAction>()
                .HasKey(x => new { x.Date, x.AdSetId, x.ActionTypeId });
            modelBuilder.Entity<FbCampaignAction>()
                .HasKey(x => new { x.Date, x.CampaignId, x.ActionTypeId });
            modelBuilder.Entity<FbReachMetric>().HasKey(x => new { x.AccountId, x.Period });
            modelBuilder.Entity<FbCampaignReachMetric>().HasKey(x => new { x.CampaignId, x.Period });
        }

        private void SetupFacebookSummaryMetricModelValues<TSummary>(DbModelBuilder modelBuilder)
            where TSummary : FbBaseSummary
        {
            modelBuilder.Entity<TSummary>().Property(t => t.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<TSummary>().Property(t => t.PostClickRev).HasPrecision(18, 6);
            modelBuilder.Entity<TSummary>().Property(t => t.PostViewRev).HasPrecision(18, 6);
        }

        private void SetupFacebookActionMetricModelValues<TAction>(DbModelBuilder modelBuilder)
            where TAction : FbAction
        {
            modelBuilder.Entity<TAction>().Property(t => t.PostClickVal).HasPrecision(18, 6);
            modelBuilder.Entity<TAction>().Property(t => t.PostViewVal).HasPrecision(18, 6);
        }

        private void SetupAdformModel(DbModelBuilder modelBuilder)
        {
            SetupAdformBaseMetricModelValues<AdfDailySummary>(modelBuilder, "AccountId");
            SetupAdformBaseMetricModelValues<AdfCampaignSummary>(modelBuilder, "CampaignId");
            SetupAdformBaseMetricModelValues<AdfLineItemSummary>(modelBuilder, "LineItemId");
            SetupAdformTrackingPointMetricModelValues(modelBuilder);
            SetupAdformBaseMetricModelValues<AdfBannerSummary>(modelBuilder, "BannerId");
        }

        private void SetupAdformBaseMetricModelValues<TBaseMetricValues>(DbModelBuilder modelBuilder, string entityColumnName)
            where TBaseMetricValues : AdfBaseSummary
        {
            modelBuilder.Entity<TBaseMetricValues>().HasKey(s => new { s.Date, s.EntityId, s.MediaTypeId });
            modelBuilder.Entity<TBaseMetricValues>().Property(x => x.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.Cost).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ClickSalesConvTypeAll).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ClickSalesConvType1).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ClickSalesConvType2).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ClickSalesConvType3).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ImpressionSalesConvTypeAll).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ImpressionSalesConvType1).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ImpressionSalesConvType2).HasPrecision(18, 6);
            modelBuilder.Entity<TBaseMetricValues>().Property(s => s.ImpressionSalesConvType3).HasPrecision(18, 6);
        }

        private void SetupAdformTrackingPointMetricModelValues(DbModelBuilder modelBuilder)
        {
            SetupAdformBaseMetricModelValues<AdfTrackingPointSummary>(modelBuilder, "TrackingPointId");
            modelBuilder.Entity<AdfTrackingPointSummary>().HasKey(s => new { s.Date, s.EntityId, s.MediaTypeId, s.LineItemId, });
        }

        private void SetupYamDailyMetricModelValues<TDailyMetricValues>(DbModelBuilder modelBuilder, string entityColumnName)
            where TDailyMetricValues : BaseYamSummary
        {
            modelBuilder.Entity<TDailyMetricValues>().HasKey(s => new { s.Date, s.EntityId });
            modelBuilder.Entity<TDailyMetricValues>().Property(x => x.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.ConversionValue).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.AdvertiserSpending).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.ClickConversionValueByPixelQuery).HasPrecision(18, 6);
            modelBuilder.Entity<TDailyMetricValues>().Property(t => t.ViewConversionValueByPixelQuery).HasPrecision(18, 6);
        }

        private void SetupSummaryMetricModel<TSummaryMetric>(DbModelBuilder modelBuilder, string entityColumnName)
            where TSummaryMetric : SummaryMetric
        {
            modelBuilder.Entity<TSummaryMetric>().HasKey(s => new { s.Date, s.EntityId, s.MetricTypeId });
            modelBuilder.Entity<TSummaryMetric>().Property(x => x.EntityId).HasColumnName(entityColumnName);
            modelBuilder.Entity<TSummaryMetric>().Property(t => t.Value).HasPrecision(18, 6);
        }
    }
}
