using CakeExtracter.Bootstrappers;
using NUnit.Framework;

namespace CakeExtracter.Tests
{
    [SetUpFixture]
    public class TestsSetUp
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            AutoMapperBootstrapper.CheckRunSetup();
        }
    }
}
