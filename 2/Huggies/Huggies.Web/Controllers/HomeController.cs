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

        public ActionResult Index(bool? test, int a = 0, int s = 0, int fill = 0)
        {
            GetSession()["a"] = a;
            GetSession()["s"] = s;
            var model = new Lead();
            if (fill == 1)
            {
                model.PopulateTestValues();
                ViewBag.Fill = true;
            }
            if (test.HasValue)
                model.Test = test.Value;
            else
#if DEBUG
                model.Test = true;
#else
                model.Test = false;
#endif
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Lead lead)
        {
            lead.SourceId = GetSession()["s"] == null ? 0 : (int)GetSession()["s"];
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