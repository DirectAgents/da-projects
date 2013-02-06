using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickBooks.Services;
using System.Linq;
using System.Collections.Generic;
using Accounting.Domain.Entities;

namespace QuickBooks.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestQBServiceLoadAndExtractCustomers()
        {
            QuickBooksSynchService service;
            service = new QuickBooksSynchService("Aaron Corp");
            service.Load(service.Extract<Customer>(), true);
        }
    }

    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestRepo()
        {
            using (var context = new AccountingContext())
            {
                var a = context.CompanyFiles;
                foreach (var item in a)
                {
                    Console.WriteLine(item.CompanyId + ", " + item.CompanyName);
                }
            }
        }
    }
}
