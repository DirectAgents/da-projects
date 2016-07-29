using System.Web.Helpers;
using System.Web.Mvc;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class ClientInfoController : CPController
    {
        public ClientInfoController(IClientPortalRepository cpRepository)
        {
            cpRepo = cpRepository;
        }

        public FileResult Logo(int id)
        {
            var clientInfo = cpRepo.GetClientInfo(id);
            if (clientInfo == null)
                return null;

            WebImage logo = new WebImage(clientInfo.Logo);
            return File(logo.GetBytes(), "image/" + logo.ImageFormat, logo.FileName);
        }
        public ActionResult EditLogo(int id = 0)
        {
            var clientInfo = cpRepo.GetClientInfo(id);
            if (clientInfo == null)
            {
                return HttpNotFound();
            }
            return View(clientInfo);
        }
        [HttpPost]
        public ActionResult UploadLogo(int id)
        {
            WebImage logo = WebImage.GetImageFromRequest();
            byte[] imageBytes = logo.GetBytes();

            var clientInfo = cpRepo.GetClientInfo(id);
            clientInfo.Logo = imageBytes;
            cpRepo.SaveChanges();

            return null;
        }
    }
}