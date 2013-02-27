using System;
using System.Linq;
using Huggies.Web.Models;
using Huggies.Web.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Huggies.Web.Test
{
    public class TestHuggiesSessionManager : ISessionManager
    {
        public void Initialize(int affiliateID)
        {
        }

        public void AbandonSession()
        {
        }

        public int GetIntValue(string key)
        {
            return 0;
        }

        public string GetStringValue(string key)
        {
            return string.Empty;
        }

        public string CurrentRequestIpAddress
        {
            get { return "::1"; }
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GoodLeadSucceeds()
        {
            var huggiesModel = new Huggies.Web.Models.HuggiesLead
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
            var homeController = new Huggies.Web.Controllers.HomeController(new TestHuggiesSessionManager(), new LeadService());
            var viewResult = (System.Web.Mvc.ViewResult)homeController.Index(huggiesModel);
            var thankYouModel = viewResult.Model as Huggies.Web.Models.ThankYouModel;

            Assert.IsTrue(thankYouModel.FirePixel);
        }

        [TestMethod]
        public void NotFirstChildFails()
        {
            var huggiesModel = new Huggies.Web.Models.HuggiesLead
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = TicksAsString() + "@x.com",
                Ethnicity = "AA",
                FirstChild = false,
                Language = "FR",
                Zip = "90210",
                Gender = "M",
                DueDate = new DateTime(2013, 4, 10)
            };
            var homeController = new Huggies.Web.Controllers.HomeController();
            var viewResult = (System.Web.Mvc.ViewResult)homeController.Index(huggiesModel);
            var thankYouModel = viewResult.Model as Huggies.Web.Models.ThankYouModel;

            // Does not fire pixel
            Assert.IsFalse(
                thankYouModel.FirePixel);

            // Has validation error that it should
            Assert.IsTrue(
                homeController.ModelState["ValidationErrors"].Errors.Any(c =>
                    c.ErrorMessage == Constants.ModelValidationError_must_be_first_child));

            // Does not have validation error that it shouldn't
            Assert.IsFalse(
                homeController.ModelState["ValidationErrors"].Errors.Any(c =>
                    c.ErrorMessage == Constants.ModelValidationError_child_must_be_lessthan_4_months_old_or_unborn));
        }

        [TestMethod]
        public void OlderThan4MonthsFails()
        {
            var huggiesModel = new Huggies.Web.Models.HuggiesLead
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = TicksAsString() + "@x.com",
                Ethnicity = "AA",
                FirstChild = true,
                Language = "FR",
                Zip = "90210",
                Gender = "M",
                DueDate = new DateTime(2012, 4, 10)
            };
            var homeController = new Huggies.Web.Controllers.HomeController();
            var viewResult = (System.Web.Mvc.ViewResult)homeController.Index(huggiesModel);
            var thankYouModel = viewResult.Model as Huggies.Web.Models.ThankYouModel;

            // Does not fire pixel
            Assert.IsFalse(
                thankYouModel.FirePixel);

            // Has validation error that it should
            Assert.IsTrue(
                homeController.ModelState["ValidationErrors"].Errors.Any(c =>
                    c.ErrorMessage == Constants.ModelValidationError_child_must_be_lessthan_4_months_old_or_unborn));

            // Does not have validation error that it shouldn't
            Assert.IsFalse(
                homeController.ModelState["ValidationErrors"].Errors.Any(c =>
                    c.ErrorMessage == Constants.ModelValidationError_must_be_first_child));
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
