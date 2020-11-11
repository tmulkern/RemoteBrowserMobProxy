using System;
using NUnit.Framework;
using RemoteBrowserMobProxy;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            RemoteBrowserMobProxy.RemoteBrowserMobProxyClient client = new RemoteBrowserMobProxyClient(new Uri("http://localhost:8080/"));
        }
    }
}