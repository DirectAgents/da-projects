using EomTool.Domain.Abstract;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class AdminController : EOMController
    {
        public AdminController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            SetAccountingPeriodViewData();
            return View();
        }

    }
}
