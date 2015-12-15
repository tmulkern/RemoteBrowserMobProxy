using System;
using NUnit.Framework;

namespace RemoteBrowserMobProxy.Tests
{
    
    public abstract class TestBase
    {
        protected RemoteBrowserMobProxyClient Client;
        protected RemoteBrowserMobProxyInstance ProxyInstance;

        public void FixtureSetup()
        {
            Client = new RemoteBrowserMobProxyClient(new Uri("http://localhost:9000/proxy"));
        }

        public void Setup()
        {
            ProxyInstance = Client.NewRemoteBrowserMobProxyInstance();
        }

        public void TearDown()
        {
            ProxyInstance.Dispose();
        }     
    }
}