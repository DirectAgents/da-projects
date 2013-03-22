using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using ClientPortal.Web.Models;
using WebMatrix.WebData;

namespace ClientPortal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

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

            WebSecurity.CreateUserAndAccount("aaron", "123456");
            WebSecurity.CreateUserAndAccount("tree", "123456", new { CakeAdvertiserId = 278 });
            WebSecurity.CreateUserAndAccount("accuquote", "123456", new { CakeAdvertiserId = 28 });
            WebSecurity.CreateUserAndAccount("amazon", "123456", new { CakeAdvertiserId = 435 });
            WebSecurity.CreateUserAndAccount("affinitas", "123456", new { CakeAdvertiserId = 298 });
            WebSecurity.CreateUserAndAccount("digital", "123456", new { CakeAdvertiserId = 67 });
            WebSecurity.CreateUserAndAccount("guthy", "123456", new { CakeAdvertiserId = 457 });
            WebSecurity.CreateUserAndAccount("scooter", "123456", new { CakeAdvertiserId = 294 });
        }
    }
}