using DirectAgents.Domain.Concrete;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DirectAgents.Domain.Test
{
    
    
    /// <summary>
    ///This is a test class for AdminImplTest and is intended
    ///to contain all AdminImplTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdminImplTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ReCreateDatabase
        ///</summary>
        [TestMethod()]
        public void ReCreateDatabaseTest()
        {
            AdminImpl target = new AdminImpl();
            target.ReCreateDatabase();
        }

        /// <summary>
        ///A test for LoadCampaigns
        ///</summary>
        [TestMethod()]
        public void LoadCampaignsTest()
        {
            AdminImpl target = new AdminImpl();
            target.LoadCampaigns();
        }
    }
}
