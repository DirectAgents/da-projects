using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTWeb.Controllers
{
    public class LTServiceController : Controller
    {
        //
        // GET: /LTService/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoIt()
        {
            return View("Call");
        }

    }

    //public class a : IView
    //{
    //    public void Render(ViewContext viewContext, System.IO.TextWriter writer)
    //    {
    //        writer.Write("foo");
    //    }
    //}
}
