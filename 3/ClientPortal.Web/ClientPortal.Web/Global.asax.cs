using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ClientPortal.Web.Models;
using WebMatrix.WebData;

namespace ClientPortal.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Database.SetInitializer(new InitSecurityDb());
            var context = new UsersContext();
            context.Database.Initialize(true);
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                         "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }
    }

    public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext userscontext)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                                                     "UserProfile", "UserId", "UserName", autoCreateTables: true);

            WebSecurity.CreateUserAndAccount("aaron", "123456", new { Culture = "en-US", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("tree", "123456", new { CakeAdvertiserId = 278, Culture = "en-US", ShowConversionRevenue = true, ConversionRevenueName = "ConvRev" });
            WebSecurity.CreateUserAndAccount("accuquote", "123456", new { CakeAdvertiserId = 28, Culture = "en-US", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("amazon", "123456", new { CakeAdvertiserId = 435, Culture = "en-US", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("affinitas", "123456", new { CakeAdvertiserId = 298, Culture = "de-DE", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("digital", "123456", new { CakeAdvertiserId = 67, Culture = "de-DE", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("guthy", "123456", new { CakeAdvertiserId = 457, Culture = "en-US", ShowConversionRevenue = false });
            WebSecurity.CreateUserAndAccount("scooter", "123456", new { CakeAdvertiserId = 294, Culture = "en-US", ShowConversionRevenue = false });

            List<Goal> goals = new List<Goal> {
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