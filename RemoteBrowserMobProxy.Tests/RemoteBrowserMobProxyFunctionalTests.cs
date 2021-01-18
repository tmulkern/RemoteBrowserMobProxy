using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace RemoteBrowserMobProxy.Tests
{
    [Ignore("Prevent this from being picked up by GitHub unit tests pipeline")]
    internal class RemoteBrowserMobProxyFunctionalTests
    {
        private IWebDriver _driver;
        private IRemoteBrowserMobProxyClient _proxyClient;

        [SetUp]
        public void Setup ()
        {
            _proxyClient = new RemoteBrowserMobProxyClient(new Uri("http://localhost:58080"));
            _proxyClient.NewRemoteBrowserMobProxyInstance(8081);

            ChromeOptions driverOptions = new ChromeOptions();
            //proxy port 8081 inside docker container is exposed to host on port 58081
            Proxy proxy = new Proxy { Kind = ProxyKind.Manual, IsAutoDetect = false, SslProxy = "http://browsermobproxy:8081" };
            driverOptions.Proxy = proxy;
            driverOptions.AddArgument("--start-maximized");
            driverOptions.AddArgument("--ignore-certificate-errors");
            _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub/"), driverOptions.ToCapabilities(), TimeSpan.FromMinutes(3));
        }


        [Test]
        public void CaptureNetworkTrafficTest()
        {
            var proxyInstance = _proxyClient.RemoteBrowserMobProxyInstances().First();
            proxyInstance.CreateHar();
            _driver.Navigate().GoToUrl(new Uri("https://hub.docker.com/"));

            // captured network traffic
            var harContent = proxyInstance.GetHarContent();
        }

        [TearDown]
        public void TearDown()
        {
            _proxyClient.RemoteBrowserMobProxyInstances().ToList().ForEach(p=>p.Dispose());
            _driver.Dispose();
        }
    }
}
