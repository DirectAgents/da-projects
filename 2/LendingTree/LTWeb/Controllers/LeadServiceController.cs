using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml.Linq;
using AutoMapper;
using LTWeb.Service.Tracking;
using LTWeb.ws;

namespace LTWeb.Controllers
{
    public class LeadServiceController : ApiController
    {
        public class LeadPostWithCallerInfo : LeadPost
        {
            public string Password { get; set; }
        }

        public HttpResponseMessage Post([FromBody] LeadPostWithCallerInfo data)
        {
            try
            {
                FixupSsn(data);

                var appId = SendToTree(data);

                TrackInCake(data, appId);

                var responseData = new XDocument(
                    new XElement("Response",
                                 new XElement("Message", "Success"),
                                 new XElement("LendingTreeGuid", appId)));

                return new HttpResponseMessage
                    {
                        Content = new StringContent(responseData.ToString(), Encoding.UTF8, "application/xml")
                    };
            }
            catch (Exception e)
            {
                var message = new StringBuilder();
                while (e != null)
                {
                    message.AppendFormat("[{0} {1}]", e.Message, e.StackTrace);
                    e = e.InnerException;
                }
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, message.ToString());
                return response;
            }
        }

        private static void FixupSsn(LeadPostWithCallerInfo data)
        {
            if (string.IsNullOrWhiteSpace(data.SSN))
                return;

            if (data.SSN == "___-__-____")
            {
                data.SSN = "";
            }
            else
            {
                var regex = new Regex("^([0-9]{3})([0-9]{2})([0-9]{4})$");
                var match = regex.Match(data.SSN);
                if (match.Success)
                    data.SSN = string.Format("{0}-{1}-{2}", match.Groups[1].Value, match.Groups[2].Value,
                                             match.Groups[3].Value);
            }
        }

        private static string SendToTree(LeadPostWithCallerInfo data)
        {
            var service = new LeadService();
            string appId = service.RefiLead(Mapper.Map<LeadPostWithCallerInfo, LeadPost>(data), data.Password);
            return appId;
        }

        private static void TrackInCake(LeadPostWithCallerInfo data, string appId)
        {
            InsertConversionParameters insertConversionParameters;
            switch (data.Password)
            {
                case "h5b5creP":
                    insertConversionParameters = new InsertConversionParameters
                    {
                        AffiliateId = 40500,
                        BillingOption = BillingOption.ignore_billing,
                        CampaignId = 13834,
                        ConversionDate = DateTime.Now.AddHours(3),
                        // adjust from west coast to east coast time
                        CreativeId = 3456,
                        Note = appId,
                        Payout = 0m,
                        Received = 0m,
                        SubAffiliate = "40500-1",
                        TotalToInsert = 1
                    };
                    break;
                case "u2s9ruvQ":
                    insertConversionParameters = new InsertConversionParameters
                    {
                        AffiliateId = 11863,
                        BillingOption = BillingOption.ignore_billing,
                        CampaignId = 15333,
                        ConversionDate = DateTime.Now.AddHours(3),
                        // adjust from west coast to east coast time
                        CreativeId = 4631,
                        Note = appId,
                        Payout = 0m,
                        Received = 0m,
                        SubAffiliate = "11863-1",
                        TotalToInsert = 1
                    };
                    break;
                default:
                    throw new Exception("Invalid Password");
            }
            var service = new TrackingService("https://login.directagents.com/api/", "FCjdYAcwQE");
            var result = service.Execute("1/track.asmx/MassConversionInsert", insertConversionParameters);
        }
    }
}