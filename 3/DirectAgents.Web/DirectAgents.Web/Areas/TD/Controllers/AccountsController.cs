using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class AccountsController : DirectAgents.Web.Controllers.ControllerBase
    {
        public AccountsController(ITDRepository tdRepository)
        {
            this.tdRepo = tdRepository;
        }

        public ActionResult Index(int? campId)
        {
            var extAccounts = tdRepo.ExtAccounts(campId: campId)
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            return View(extAccounts);
        }
	}
}