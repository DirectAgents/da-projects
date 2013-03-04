using System;
using System.Web;
using System.Web.Mvc;
using Huggies.Web.Models;
using Huggies.Web.Services;
using KimberlyClark.Services.Abstract;

namespace Huggies.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IService _service;

        public HomeController()
        {
            _service = new Service();
        }

        public HomeController(IService ls, HttpSessionStateBase ssb, HttpRequestBase rq, ModelStateDictionary msd)
            : base(ssb, rq, msd)
        {
            _service = ls;
        }

        public ActionResult Index(int a = 0, int test = 0)
        {
            GetSession()["a"] = a;
            var model = new Lead();
            if (test == 1)
            {
                model.PopulateTestValues();
                ViewBag.TestMode = true;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Lead lead)
        {
            var model = new ThankYou();
            if (lead.Validate(ModelState))
            {
                IProcessResult result;
                if (_service.SendLead(lead, out result))
                {
                    if (result.IsSuccess)
                    {
                        model.LeadId = lead.Id;
                        lead.Success = true;
                        model.FirePixel = true;
                    }
                }
            }
            lead.Timestamp = DateTime.Now;
            lead.AffiliateId = GetSession()["a"] == null ? 0 : (int) GetSession()["a"];
            lead.IpAddress = GetRequest().UserHostAddress;
            _service.SaveLead(lead, GetModelState()["ValidationErrors"].ToStringArray());
            return View(ViewNames.Home.ThankYou, model);
        }
    }
}