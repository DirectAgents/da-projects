using System.Text.RegularExpressions;
using CakeExtracter.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CakeExtracter.UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void copy_a_to_a()
        {
            A source = new A
                {
                    AInt = 1,
                    AString = "s",
                    Rx = new Regex("foo"),
                    B = new B
                        {
                            BInt = 2
                        }
                };
            A target = new A();
            CopyPropertiesUtility.Copy(source, target);
            Assert.AreEqual(source.AInt, target.AInt);
            Assert.AreEqual(source.AString, target.AString);
            Assert.AreEqual(source.B.BInt, target.B.BInt);
            Assert.AreEqual(source.Rx, target.Rx);
        }

        [TestMethod]
        public void clone_a()
        {
            A source = new A
            {
                AInt = 1,
                AString = "s",
                Rx = new Regex("foo"),
                B = new B
                {
                    BInt = 2
                }
            };
            A target = CopyPropertiesUtility.Clone(source);
            Assert.AreEqual(source.AInt, target.AInt);
            Assert.AreEqual(source.AString, target.AString);
            Assert.AreEqual(source.B.BInt, target.B.BInt);
            Assert.AreEqual(source.Rx, target.Rx);
        }

        [TestMethod]
        public void copy_a_to_c()
        {
            A source = new A
            {
                AInt = 1,
                AString = "s",
                Rx = new Regex("foo"),
                B = new B
                {
                    BInt = 2
                }
            };
            C target = new C {CString = "nochange"};
            CopyPropertiesUtility.Copy(source, target);
            Assert.AreEqual(source.AInt, target.AInt);
            Assert.AreEqual("s", source.AString);
            Assert.AreEqual(source.B.BInt, target.B.BInt);
            Assert.IsNotNull(source.Rx);
            Assert.AreEqual("nochange", target.CString);
        }

        class A
        {
            public int AInt { get; set; }
            public B B { get; set; }
            public string AString { get; set; }
            public Regex Rx { get; set; }
        }

        class B
        {
            public int BInt { get; set; }
        }

        class C
        {
            public int AInt { get; set; }
            public B B { get; set; }
            public string CString { get; set; }
        }
    }
}
