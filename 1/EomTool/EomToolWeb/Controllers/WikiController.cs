using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class WikiController : Controller
    {
        [HttpGet]
        public ActionResult Settings()
        {
            var model = SessionUtility.WikiSettings;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Settings(WikiSettings wikiSettings)
        {
            var settings = SessionUtility.WikiSettings;
            if (ModelState.IsValid)
            {
                TryUpdateModel(settings);
            }

            if (Request.IsAjaxRequest())
                return Json(new { IsValid = ModelState.IsValid });
            else
                return PartialView(settings);
        }
    }
}
