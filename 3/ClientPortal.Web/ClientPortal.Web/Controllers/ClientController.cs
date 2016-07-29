using System.Web.Helpers;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;

namespace ClientPortal.Web.Controllers
{
    [Authorize]
    public class ClientController : CPController
    {
        public ClientController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public FileResult Logo()
        {
            var userInfo = GetUserInfo();
            if (userInfo.ClientLogo == null)
                return null;

            WebImage logo = new WebImage(userInfo.ClientLogo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }
    }
}