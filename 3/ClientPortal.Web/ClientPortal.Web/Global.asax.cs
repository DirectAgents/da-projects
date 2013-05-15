using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ClientPortal.Web.Models;
using StackExchange.Profiling;
using WebMatrix.WebData;
using System.Data.Entity.Migrations;

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

            // add service master beta account if it does not exist
            if (!WebSecurity.UserExists("sm"))
            {
                WebSecurity.CreateUserAndAccount(
                    "sm",
                    "123456",
                    new
                    {
                        CakeAdvertiserId = 207,
                        Culture = "en-US",
                        ShowCPMRep = false,
                        ShowConversionData = false,
                        ConversionValueIsNumber = false
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
                        Culture = "en-US",
                        ShowCPMRep = false,
                        ShowConversionData = false,
                        ConversionValueIsNumber = false
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
                        Culture = "en-US",
                        ShowCPMRep = false,
                        ShowConversionData = false,
                        ConversionValueIsNumber = false
                    });
            }

            // add goals for beta users
            using (var db = new UsersContext())
            {
                Goal[] goals = new[] {
                    // sm goals
                    new Goal() { AdvertiserId = 207, OfferId = 1927, MetricId = MetricEnum.Leads, Target = 850, TypeId = GoalTypeEnum.Absolute, Name = "reach 850 leads in month" },
                    new Goal() { AdvertiserId = 207, OfferId = 1927, MetricId = MetricEnum.Spend, Target = 12000, TypeId = GoalTypeEnum.Absolute, Name = "spend 12000" },
                };
                db.Goals.AddOrUpdate(goals);
                db.SaveChanges();
            }
        }
    }

    public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext userscontext)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                     "UserProfile", "UserId", "UserName", autoCreateTables: true);

            // For testing
            WebSecurity.CreateUserAndAccount("affinitas", "123456", new { CakeAdvertiserId = 298, Culture = "de-DE", ShowCPMRep = false, ShowConversionData = true, ConversionValueName = "ValTest", ConversionValueIsNumber = false });
            WebSecurity.CreateUserAndAccount("guthy", "123456", new { CakeAdvertiserId = 457, Culture = "en-US", ShowCPMRep = true, ShowConversionData = false, ConversionValueIsNumber = false });

            // Beta customers
            WebSecurity.CreateUserAndAccount("tree", "123456", new { CakeAdvertiserId = 278, Culture = "en-US", ShowCPMRep = false, ShowConversionData = true, ConversionValueIsNumber = true }); // lending tree
            WebSecurity.CreateUserAndAccount("bas", "123456", new { CakeAdvertiserId = 455, Culture = "en-US", ShowCPMRep = false, ShowConversionData = false, ConversionValueIsNumber = false }); // bryant and stratten

            List<Goal> goals = new List<Goal> {

                // lending tree goals
                new Goal() { AdvertiserId = 278, OfferId = 11993, MetricId = MetricEnum.Leads, Target = 2000, TypeId = GoalTypeEnum.Absolute, Name = "new form leads" },
                new Goal() { AdvertiserId = 278, OfferId = 11993, MetricId = MetricEnum.Spend, Target = 100000, TypeId = GoalTypeEnum.Absolute, Name = "new form spend" },
                new Goal() { AdvertiserId = 278, OfferId = 1734, MetricId = MetricEnum.Clicks, Target = 10.5m, TypeId = GoalTypeEnum.Percent, Name = "ssn clicks pct" },
                new Goal() { AdvertiserId = 278, OfferId = 1734, MetricId = MetricEnum.Spend, Target = 5, TypeId = GoalTypeEnum.Percent, Name = "ssn spend pct" },
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = MetricEnum.Spend, Target = 50, TypeId = GoalTypeEnum.Absolute, Name = "tr edarling spend abs" },
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = MetricEnum.Spend, Target = 10, TypeId = GoalTypeEnum.Percent, Name = "tr edarling spend pct" },
                new Goal() { AdvertiserId = 298, OfferId = 1618, MetricId = MetricEnum.Clicks, Target = 6000, TypeId = GoalTypeEnum.Absolute, Name = "tr edarling clicks" }

            };
            foreach (var goal in goals)
            {
                userscontext.Goals.Add(goal);
            }
            userscontext.SaveChanges();
        }
    }
}