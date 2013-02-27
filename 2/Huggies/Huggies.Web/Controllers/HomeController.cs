using System;
using System.Linq;
using System.Web.Mvc;
using Huggies.Web.Models;
using Huggies.Web.Services;
using KimberlyClark.Services.Abstract;

namespace Huggies.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            Service = new Service();
        }

        public HomeController(IService ls)
        {
            Service = ls;
        }

        public IService Service { get; set; }

        private string[] ValidationErrors
        {
            get
            {
                return ModelState["ValidationErrors"] == null
                           ? new string[] {}
                           : ModelState["ValidationErrors"].Errors.Select(c => c.ErrorMessage).ToArray();
            }
        }

        public ActionResult Index(int a = 0, int test = 0)
        {
            Session["a"] = a;
            var model = new Lead();
            if (test == 1)
            {
                PopulateTestValues(model);
                ViewBag.TestMode = true;
            }
            return View(model);
        }

        private void PopulateTestValues(Lead model)
        {
            model.FirstName = "Jane";
            model.LastName = "Doe";
            model.Email = TicksAsString() + "@x.com";
            model.Ethnicity = "AA";
            model.FirstChild = true;
            model.Language = "FR";
            model.Zip = "90210";
            model.Gender = "M";
            model.DueDate = new DateTime(2013, 4, 10);
        }

        private string TicksAsString()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                               .Replace('+', '_')
                               .Replace('/', '-')
                               .TrimEnd('=');
            return id;
        }

        [HttpPost]
        public ActionResult Index(Lead lead)
        {
            var model = new ThankYou();
            if (lead.Validate(ModelState))
            {
                IProcessResult result;
                if (Service.SendLead(lead, out result) && result.IsSuccess)
                {
                    model.LeadId = lead.Id;
                    lead.Success = true;
                    model.FirePixel = true;
                }
            }

            lead.Timestamp = DateTime.Now;
            lead.AffiliateId = Session["a"] == null ? 0 : (int)Session["a"];
            lead.IpAddress = Request.UserHostAddress;
            Service.SaveLead(lead, ValidationErrors);

            return View(ViewNames.Home.ThankYou, model);
        }
    }
}