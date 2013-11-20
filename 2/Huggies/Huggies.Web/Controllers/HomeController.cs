using System;
using System.Web;
using System.Web.Mvc;
using Huggies.Web.Models;
using Huggies.Web.Services;
using KimberlyClark.Services.Abstract;
using RestSharp;
using KimberlyClark.Services.Concrete;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

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
            model.SetTest(test);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Lead lead)
        {
            lead.AffiliateId = GetSession()["a"] == null ? 0 : (int)GetSession()["a"];
            lead.SourceId = GetSession()["s"] == null ? 0 : (int)GetSession()["s"];
            var model = SendAndSaveLead(lead);

            return View(ViewNames.Home.ThankYou, model);
        }

        public JsonResult Submit(Lead lead, bool? test)
        {
            lead.SetTest(test);
            if (lead.AffiliateId == 0)
                lead.AffiliateId = -1;

            var model = SendAndSaveLead(lead);

            return Json(lead.Success, JsonRequestBehavior.AllowGet);
        }

        private ThankYou SendAndSaveLead(Lead lead)
        {
            lead.Success = false;
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
            lead.IpAddress = GetRequest().UserHostAddress;
            _service.SaveLead(lead, GetModelState()["ValidationErrors"].ToStringArray());

            return model;
        }

        public JsonResult Test(int? s, int x = 0, bool test = true)
        {
            if (x == 123)
            {
                var lead = new Lead();
                lead.PopulateTestValues();
                lead.Gender = "M";
                if (s.HasValue)
                    lead.SourceId = s.Value;
                lead.Test = test;
                IRestResponse<ProcessResult> restResponse;
                IConsumer consumer;
                _service.SendLead(lead, out restResponse, out consumer);

                var testResponse = new TestResponse(consumer, restResponse);
                var json = Json(testResponse, JsonRequestBehavior.AllowGet);
                return json;
            }
            else
            {
                return Json("test", JsonRequestBehavior.AllowGet);
            }
        }

        public class TestResponse
        {
            public TestResponse(IConsumer consumer, IRestResponse<ProcessResult> restResponse)
            {
                Consumer = consumer;

                Content = restResponse.Content;
                ContentEncoding = restResponse.ContentEncoding;
                ContentLength = restResponse.ContentLength;
                ContentType = restResponse.ContentType;
                Cookies = restResponse.Cookies;
                ErrorException = restResponse.ErrorException;
                ErrorMessage = restResponse.ErrorMessage;
                Headers = restResponse.Headers;
                //RawBytes = restResponse.RawBytes;
                //Request = restResponse.Request;
                ResponseStatus = restResponse.ResponseStatus;
                ResponseUri = restResponse.ResponseUri;
                Server = restResponse.Server;
                StatusCode = restResponse.StatusCode;
                StatusDescription = restResponse.StatusDescription;

                ProcessResult = restResponse.Data;
            }

            public string Content { get; set; }
            public string ContentEncoding { get; set; }
            public long ContentLength { get; set; }
            public string ContentType { get; set; }
            public IList<RestResponseCookie> Cookies { get; set; }
            public Exception ErrorException { get; set; }
            public string ErrorMessage { get; set; }
            public IList<Parameter> Headers { get; set; }
            public byte[] RawBytes { get; set; }
            public IRestRequest Request { get; set; }
            public ResponseStatus ResponseStatus { get; set; }
            public Uri ResponseUri { get; set; }
            public string Server { get; set; }
            public HttpStatusCode StatusCode { get; set; }
            public string StatusDescription { get; set; }

            public ProcessResult ProcessResult { get; set; }

            public IConsumer Consumer { get; set; }
        }
    }
}