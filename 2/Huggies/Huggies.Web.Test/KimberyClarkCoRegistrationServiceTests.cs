using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using KimberlyClark.Services.Abstract;
using KimberlyClark.Services.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

//using RestSharp.Serializers;

namespace Huggies.Web.Test
{
    /// <summary>
    /// Summary description for UnitTest2
    /// </summary>
    [TestClass]
    public class KimberyClarkCoRegistrationServiceTests
    {
        public KimberyClarkCoRegistrationServiceTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CheckIfConsumerExistsCanBeCalled()
        {
            var service = GetKimberlyClarkCoRegistrationRestService();
            bool exists = service.CheckIfConsumerExists("lalalalal@x.com");
            Console.WriteLine(exists);
        }

        private static IKimberlyClarkCoRegistrationRestService GetKimberlyClarkCoRegistrationRestService()
        {
#if DEBUG
            var service = new KimberlyClarkCoRegistrationRestService(
                "https://www.qa.coregistrationservice.kimberly-clark.com/CoRegistrationRestService",
                "kcinet\\inustcap04",
                "Happy1234567");
#else
            IKimberlyClarkCoRegistrationRestService service = new KimberlyClarkCoRegistrationRestService(
                "https://www.coregistrationservice.kimberly-clark.com/CoRegistrationRestService",
                "kcinet\\inustcap08",
                "BU784562qzp");
#endif
            return service;
        }

        [TestMethod]
        public void ProcessConsumerInformationCanBeCalled()
        {
            var service = GetKimberlyClarkCoRegistrationRestService();

            IConsumer consumer = new Consumer();
            consumer.FirstName = "Jane";
            consumer.LastName = "Doe";
            consumer.EmailAddress = TicksAsString() + "@x.com";
            consumer.EthnicityCode = "AA";
            consumer.FirstTimeParentFlag = true;
            consumer.FirstTimeParentFlagSpecified = true;
            consumer.LanguageCode = "FR";
            consumer.PostalCode = "90210";
            consumer.ConsumerChild = new[]
            {
                new Child
                    {
                        BirthDate =DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy"), 
                        Gender = "M"
                    }
            };
            consumer.Country = "US";
            consumer.BrandID = 6;
            consumer.BrandIDSpecified = true;
            consumer.SourceID = 637;
            consumer.SourceIDSpecified = true;
            consumer.VendorID = 2;
            consumer.VendorIDSpecified = true;
            consumer.DMPermission = true;
            consumer.DMPermissionSpecified = true;
            consumer.EmailPermission = true;
            consumer.EmailPermissionSpecified = true;
            consumer.MobilePermission = false;
            consumer.MobilePermissionSpecified = true;
            consumer.KCBrandsDMPermission = true;
            consumer.KCBrandsDMPermissionSpecified = true;
            consumer.KCBrandsEmailpermission = true;
            consumer.KCBrandsEmailpermissionSpecified = true;
            consumer.ThirdPartyDMPermission = true;
            consumer.ThirdPartyDMPermissionSpecified = true;
            consumer.ThirdPartyEmailpermission = true;
            consumer.ThirdPartyEmailpermissionSpecified = true;

           // var doc = SerializeToXDocument(consumer);
           // RemoveNillElements(doc);

            Console.WriteLine(consumer.ToString());

            var result = service.ProcessConsumerInformation(consumer);

            Console.WriteLine(result.ToString());
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
