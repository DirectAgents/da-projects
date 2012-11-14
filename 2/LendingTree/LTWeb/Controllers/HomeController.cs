using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int? mode, bool? debug)
        {
            var sessionModel = InitializeSessionModel();

            // mode=1 --> SSN is required
            // mode=2 --> SSN is not required
            if (mode != null)
            {
                if (mode == 1)
                {
                    sessionModel.SsnRequired = true;
                }
                else if (mode == 2)
                {
                    sessionModel.SsnRequired = false;
                }
            }

            if (debug != null)
            {
            }

            return RedirectToAction("Show", "Questions");
        }

        private ILendingTreeModel InitializeSessionModel()
        {
            ILendingTreeModel sessionModel = new LendingTreeModel("Test"); // TODO: get service config name from somewhere else
            sessionModel.Initialize();
            Session["LTModel"] = sessionModel;
            return sessionModel;
        }
    }
}
