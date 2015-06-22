using System.Web.Mvc;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected IMainRepository mainRepo;
        protected ISecurityRepository securityRepo;


        // TODO: Make SecurityRepo disposable and dispose here:

        protected override void Dispose(bool disposing)
        {
            mainRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}