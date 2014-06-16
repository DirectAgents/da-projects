using EomTool.Domain.Abstract;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class AdminController : EOMController
    {

        public AdminController(ISecurityRepository securityRepository)
        {
            this.securityRepo = securityRepository;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            return View();
        }

    }
}
