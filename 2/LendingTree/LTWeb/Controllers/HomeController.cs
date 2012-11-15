using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LTWeb.DataAccess;
using LTWeb.Service;

namespace LTWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? mode, bool? debug)
        {
            using (var db = new LTWebDataContext()) // TDOO: dependency injection
                Settings.Reset(db);

            var ltModel = Settings.LTModel;

            // mode=1 --> SSN is required
            // mode=2 --> SSN is not required
            if (mode != null)
            {
                if (mode == 1)
                {
                    ltModel.SsnRequired = true;
                }
                else if (mode == 2)
                {
                    ltModel.SsnRequired = false;
                }
            }

            if (debug != null) // TODO
            {
            }

            return RedirectToAction("Show", "Questions");
        }
    }
}
