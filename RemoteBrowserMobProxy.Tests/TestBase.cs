using System;

namespace RemoteBrowserMobProxy.Tests
{
    
    public abstract class TestBase
    {
        protected RemoteBrowserMobProxyClient Client;
        protected RemoteBrowserMobProxyInstance ProxyInstance;

        public virtual void FixtureSetup()
        {
            Client = new RemoteBrowserMobProxyClient(new Uri("http://localhost:9000/proxy"));
        }

        public virtual void Setup()
        {
            ProxyInstance = Client.NewRemoteBrowserMobProxyInstance();
        }

        public virtual void TearDown()
        {
            ProxyInstance.Dispose();
        }     
    }
}