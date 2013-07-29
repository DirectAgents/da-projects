using System;
using System.IO;
using System.Linq;
using System.Text;
using ClientPortal.Data.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientPortal.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ClicksByDevice(278, new DateTime(2013, 7, 1), new DateTime(2013, 7, 27))
                               .OrderByDescending(c => c.ClickCount)
                               .ToList();
                var sb = new StringBuilder();
                var tw = new StringWriter(sb);
                var serializer = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
                serializer.Serialize(tw, result);
                Console.WriteLine(sb);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ConversionsByRegion(278, new DateTime(2013, 7, 1), new DateTime(2013, 7, 27))
                               .ToList();
                Console.WriteLine(result.ToArray().ToJson());

            }
        }
    }
    static class Ext
    {
        public static string ToJson(this object[] array)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var serializer = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            });
            serializer.Serialize(tw, array);
            return sb.ToString();
        }
    }
}
