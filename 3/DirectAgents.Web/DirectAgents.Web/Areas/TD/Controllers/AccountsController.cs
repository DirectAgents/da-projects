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

        public ActionResult Index()
        {
            var accounts = tdRepo.Accounts()
                .OrderBy(a => a.Platform.Name).ThenBy(a => a.Name);

            return View(accounts);
        }
	}
}