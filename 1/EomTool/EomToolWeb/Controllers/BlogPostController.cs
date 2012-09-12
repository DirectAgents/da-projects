using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class BlogPostController : Controller
    {
        public ActionResult Create() 
        {
            return View(); 
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create_post(BlogPost model) 
        {
            ViewBag.HtmlContent = model.Content; 
            return View(model); 
        }
    }
}
