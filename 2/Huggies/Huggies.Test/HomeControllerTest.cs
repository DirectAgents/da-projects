using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Huggies.Web.Controllers;
using Huggies.Web.Models;
using Huggies.Web.Services;
using KimberlyClark.Services.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Huggies.Test
{
    // until I get more comfortable with the terms "mock, fake and stuf" I'm using "My" as a generic catch-all

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

    public class MySession : HttpSessionStateBase
    {
        private readonly IDictionary<object, object> dic = new Dictionary<object, object>();

        public override object this[string name]
        {
            get { return dic.ContainsKey(name) ? dic[name] : null; }
            set { dic[name] = value; }
        }
    }

    public class MyRequest : HttpRequestBase
    {
        public override string UserHostAddress
        {
            get { return "::1"; }
        }
    }

    public class MyLead : Lead
    {
        public bool ValidateCalled { get; set; }
        public bool ValidateReturns { get; set; }

        public override bool Validate(ModelStateDictionary modelState)
        {
            ValidateCalled = true;
            return ValidateReturns;
        }
    }

    /// <summary>
    /// Lead is valid or invalid
    /// If Lead is valid
    ///   Lead is sent or not
    ///   If Lead is sent
    ///      Result is success or not
    ///      If Result is success
    ///         model.LeadId = lead.Id;
    ///         lead.Success = true;
    ///         model.FirePixel = true;
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_when_modelstate_invalid()
        {
            // Arrange
            //
            var inputModel = new MyLead
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = Guid.NewGuid() + "@x.com",
                    Ethnicity = "AA",
                    FirstChild = true,
                    Language = "FR",
                    Zip = "90210",
                    Gender = "N",
                    DueDate = new DateTime(2013, 2, 10),

                    ValidateReturns = false // invalid lead
                };
            var service = new MyService();
            var sessionState = new MySession();
            var httpRequest = new MyRequest();
            var modelState = new ModelStateDictionary();
            var controller = new HomeController(service, sessionState, httpRequest, modelState);

            // Act
            //
            var viewResult = (ViewResult) controller.Index(inputModel);
            var outputModel = (ThankYou) viewResult.Model;

            // Assert
            //
            Assert.IsTrue(inputModel.ValidateCalled);
            Assert.IsFalse(outputModel.FirePixel);
        }
    }
}