using System.Net;
using NUnit.Framework;
using RemoteBrowserMobProxy.Data;

namespace RemoteBrowserMobProxy.Tests
{
    [TestFixture]
    public class TestHarFeatures : TestBase
    {
        [OneTimeSetUp]
        public override void FixtureSetup()
        {
            base.FixtureSetup();
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }  


        [Test]
        public void CreateNewHar()
        {
            ProxyInstance.CreateHar(new HarOptions());
        }

        [Test]
        public void GetHarContent()
        {
            ProxyInstance.CreateHar(new HarOptions());

            using (var wc = new WebClient())
            {
                wc.Proxy = new WebProxy("localhost",ProxyInstance.Port);

                wc.DownloadString("http://www.google.ie");
            }


            var data=ProxyInstance.GetHarContent(new HarOptions());
        }
    }
}