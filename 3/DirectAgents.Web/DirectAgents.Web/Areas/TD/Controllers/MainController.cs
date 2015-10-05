using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DirectAgents.Web.Areas.TD.Controllers
{
    public class MainController : DirectAgents.Web.Controllers.ControllerBase
    {
        public ActionResult ChooseMonth(DateTime month)
        {
            CurrentMonthTD = month;
            return Redirect(Request.UrlReferrer.ToString());
        }
	}
}