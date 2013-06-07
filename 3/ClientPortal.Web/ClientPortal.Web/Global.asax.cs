using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using ClientPortal.Data.DTOs;
using ClientPortal.Web.Models;
using StackExchange.Profiling;
using WebMatrix.WebData;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Services;

namespace ClientPortal.Web
{
    public class MvcApplication : HttpApplication
    {
        public static bool UseMiniProfiler
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["UseMiniProfiler"]); }
        }

        public static bool UseBlackbird
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["UseBlackbird"]); }
        }

        protected void Application_BeginRequest()
        {
            if (UseMiniProfiler)
                MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            if (UseMiniProfiler)
                MiniProfiler.Stop();
        }

        protected void Application_Start()
        {
            if (UseMiniProfiler)
            {
                MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
                MiniProfiler.Settings.PopupToggleKeyboardShortcut = "F4";
                MiniProfiler.Settings.PopupStartHidden = true;
                MiniProfilerEF.Initialize();
            }

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            if (bool.Parse(ConfigurationManager.AppSettings["UseNullDatabaseInitializer"]))
            {
                Database.SetInitializer<UsersContext>(null);
            }
            else
            {
                Database.SetInitializer(new InitSecurityDb());
                var context = new UsersContext();
                context.Database.Initialize(true);
            }

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                         "UserProfile", "UserId", "UserName", autoCreateTables: true);

            // Configure AutoMapper
            Mapper.CreateMap<OfferInfo, OfferSummaryReportExportRow>()
                .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Leads, opt => opt.MapFrom(src => src.Conversions))
                .ForMember(dest => dest.Spend, opt => opt.MapFrom(src => src.Revenue));
            Mapper.CreateMap<DailyInfo, DailySummaryReportExportRow>()
                .ForMember(dest => dest.Leads, opt => opt.MapFrom(src => src.Conversions))
                .ForMember(dest => dest.Spend, opt => opt.MapFrom(src => src.Revenue))
                .ForMember(dest => dest.SpendPerClick, opt => opt.MapFrom(src => src.EPC));
            Mapper.CreateMap<ConversionInfo, ConversionReportExportRow>()
                .ForMember(dest => dest.SubId, opt => opt.MapFrom(src => src.AffId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceReceived));
            Mapper.CreateMap<AffiliateSummary, AffiliateReportExportRow>()
                .ForMember(dest => dest.SubId, opt => opt.MapFrom(src => src.AffId))
                .ForMember(dest => dest.Leads, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceReceived));

            // add admin account if it does not exist
            if (!WebSecurity.UserExists("admin"))
            {
                WebSecurity.CreateUserAndAccount(
                    "admin",
                    "da@dmin",
                    new
                    {
                        CakeAdvertiserId = 0,
                    });
            }

            // add select quote dummy account if it does not exist
            if (!WebSecurity.UserExists("selectquote"))
            {
                WebSecurity.CreateUserAndAccount(
                    "selectquote",
                    "123456",
                    new
                    {
                        CakeAdvertiserId = 580,
                    });
            }

            // add service master beta account if it does not exist
            if (!WebSecurity.UserExists("sm"))
            {
                WebSecurity.CreateUserAndAccount(
                    "sm",
                    "123456",
                    new
                    {
                        CakeAdvertiserId = 207,
                    });
            }

            // add amazon local beta account if it does not exist
            if (!WebSecurity.UserExists("amazon"))
            {
                WebSecurity.CreateUserAndAccount(
                    "amazon",
                    "123456",
                    new
                    {
                        CakeAdvertiserId = 435,
                    });
            }

            // add itt beta account if it does not exist
            if (!WebSecurity.UserExists("itt"))
            {
                WebSecurity.CreateUserAndAccount(
                    "itt",
                    "123456",
                    new
                    {
                        CakeAdvertiserId = 250,
                    });
            }

            var cpRepo = new ClientPortalRepository(new ClientPortalContext());
            var existingGoals = cpRepo.Goals;

            // add goals for beta users
            Goal[] goalsToAdd = new[] {
                // sm goals
                new Goal() { AdvertiserId = 207, OfferId = 1927, MetricId = Metric.Leads, Target = 850, TypeId = GoalType.Absolute, Name = "reach 850 leads in month" },
                new Goal() { AdvertiserId = 207, OfferId = 1927, MetricId = Metric.Spend, Target = 12000, TypeId = GoalType.Absolute, Name = "spend 12000" },
            };
            foreach (var goal in goalsToAdd)
            {
                if (!existingGoals.Any(g => g.AdvertiserId == goal.AdvertiserId &&
                                            g.OfferId == goal.OfferId &&
                                            g.MetricId == goal.MetricId &&
                                            g.Target == goal.Target &&
                                            g.TypeId == goal.TypeId))
                {
                    cpRepo.AddGoal(goal);
                }
                cpRepo.SaveChanges();
            }

            Seeder.Seed(cpRepo);
        }
    }

    public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext userscontext)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                     "UserProfile", "UserId", "UserName", autoCreateTables: true);

            // For testing
            WebSecurity.CreateUserAndAccount("affinitas", "123456", new { CakeAdvertiserId = 298 });
            WebSecurity.CreateUserAndAccount("guthy", "123456", new { CakeAdvertiserId = 457 });

            // Beta customers
            WebSecurity.CreateUserAndAccount("tree", "123456", new { CakeAdvertiserId = 278 }); // lending tree
            WebSecurity.CreateUserAndAccount("bas", "123456", new { CakeAdvertiserId = 455 }); // bryant and stratten

            List<Goal> goals = new List<Goal> {

                // select quote goals
                new Goal() { AdvertiserId = 530, OfferId = 30389, MetricId = Metric.Leads, Target = 1000, TypeId = GoalType.Absolute, Name = "1000 leads" },

                // lending tree goals
                new Goal() { AdvertiserId = 278, OfferId = 11993, MetricId = Metric.Leads, Target = 2000, TypeId = GoalType.Absolute, Name = "new form leads" },
                new Goal() { AdvertiserId = 278, OfferId = 11993, MetricId = Metric.Spend, Target = 100000, TypeId = GoalType.Absolute, Name = "new form spend" },
                new Goal() { AdvertiserId = 278, OfferId = 1734, MetricId = Metric.Clicks, Target = 10.5m, TypeId = GoalType.Percent, Name = "ssn clicks pct" },
                new Goal() { AdvertiserId = 278, OfferId = 1734, MetricId = Metric.Spend, Target = 5, TypeId = GoalType.Percent, Name = "ssn spend pct" },

                // edarling goals (remove these?)
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = Metric.Spend, Target = 50, TypeId = GoalType.Absolute, Name = "tr edarling spend abs" },
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = Metric.Spend, Target = 10, TypeId = GoalType.Percent, Name = "tr edarling spend pct" },
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = Metric.Clicks, Target = 6000, TypeId = GoalType.Absolute, Name = "tr edarling clicks" },

            };

            var cpRepo = new ClientPortalRepository(new ClientPortalContext());
            foreach (var goal in goals)
            {
                cpRepo.AddGoal(goal);
            }
            cpRepo.SaveChanges();
        }
    }
}