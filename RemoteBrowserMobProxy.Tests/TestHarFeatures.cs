using System.Net;
using Flurl.Http.Testing;
using NUnit.Framework;
using RemoteBrowserMobProxy.DataClasses;

namespace RemoteBrowserMobProxy.Tests
{
    [TestFixture]
    public class TestHarFeatures : TestBase
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            base.FixtureSetup();
        }

        [SetUp]
        public void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public void TearDown()
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