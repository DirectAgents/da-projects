using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ClientPortal.Web.Areas.Admin.Controllers
{
    [Authorize(Users = "admin")]
    public class CreativeFilesController : CPController
    {
        public CreativeFilesController(IClientPortalRepository cpRepository)
        {
            this.cpRepo = cpRepository;
        }

        public ActionResult Index(int? creativeid)
        {
            var creativeFiles = cpRepo.CreativeFiles(creativeid);
            return View(creativeFiles.OrderBy(cf => cf.CreativeFileName));
        }

    }
}
