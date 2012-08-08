using EomApp1.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EomApp1.Test
{
    
    
    /// <summary>
    ///This is a test class for CurrentUserTest and is intended
    ///to contain all CurrentUserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CurrentUserTest
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
        ///A test for CanFinalize
        ///</summary>
        [TestMethod()]
        public void CanFinalizeTest()
        {
            string accountManagerName = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = CurrentUser.CanFinalize(accountManagerName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CanVerify
        ///</summary>
        [TestMethod()]
        public void CanVerifyTest()
        {
            bool actual;
            actual = CurrentUser.CanVerify;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PermissionTags
        ///</summary>
        [TestMethod()]
        public void PermissionTagsTest()
        {
            List<string> actual;
            actual = CurrentUser.PermissionTags;
            if (DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityNameLower() == "aaron-pavillion\\aaron")
            {
                foreach (var item in actual)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Assert.Inconclusive("Verify the correctness of this test method.");
            }
        }

        /// <summary>
        ///A test for RoleIds
        ///</summary>
        [TestMethod()]
        public void RoleIdsTest()
        {
            List<int> actual;
            actual = CurrentUser.RoleIds;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RoleNames
        ///</summary>
        [TestMethod()]
        public void RoleNamesTest()
        {
            List<string> actual;
            actual = CurrentUser.RoleNames;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Roles
        ///</summary>
        [TestMethod()]
        public void RolesTest()
        {
            List<Role> actual;
            actual = CurrentUser.Roles;
            if (DAgents.Common.WindowsIdentityHelper.GetWindowsIdentityNameLower() == "aaron-pavillion\\aaron")
            {
                foreach (var item in actual)
                {
                    Console.WriteLine(item.Name);
                }
            }
            else
            {
                Assert.Inconclusive("Verify the correctness of this test method.");
            }
        }
    }
}
