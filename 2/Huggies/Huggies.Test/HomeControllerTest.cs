using System;
using System.Web.Mvc;
using Huggies.Web.Controllers;
using Huggies.Web.Models;
using Huggies.Web.Services;
using KimberlyClark.Services.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Huggies.Test
{
    // Note: until I get more comfortable with the terms "mock, fake and stuf" I'm using "My" as a generic catch-all
    public class MyService : IService
    {
        public bool SendLead(ILead lead, out IProcessResult processResult)
        {
            Console.WriteLine("SendLead()");
            processResult = null;
            return false;
        }

        public void SaveLead(ILead lead, string[] validationErrors)
        {
            Console.WriteLine("SaveLead()");
            if (validationErrors != null)
            {
                foreach (var validationError in validationErrors)
                {
                    Console.WriteLine("Validation Error: {0}", validationError);
                }
            }
        }
    }

    internal enum WaysToInvalidateLead
    {
        NotFirstChild,
        ChildTooOld
    }

    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var model = GetLead();
            var service = GetService();
            var controller = new HomeController(service);
            var viewResult = (ViewResult) controller.Index(model);
            //var thankYouModel = viewResult.Model as Huggies.Web.Models.ThankYouModel;
            //Assert.IsTrue(thankYouModel.FirePixel);
        }

        private IService GetService()
        {
            return new MyService();
        }

        private Lead LeadGetLead(WaysToInvalidateLead ways)
        {
            var lead = GetLead();
            switch (ways)
            {
                case WaysToInvalidateLead.NotFirstChild:
                    lead.FirstChild = false;
                    break;
                case WaysToInvalidateLead.ChildTooOld:
                    lead.DueDate = DateTime.Now.AddMonths(5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ways");
            }
            return lead;
        }

        private Lead GetLead()
        {
            return new Lead
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = TicksAsString() + "@x.com",
                    Ethnicity = "AA",
                    FirstChild = true,
                    Language = "FR",
                    Zip = "90210",
                    Gender = "M",
                    DueDate = new DateTime(2013, 4, 10)
                };
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
    }
}