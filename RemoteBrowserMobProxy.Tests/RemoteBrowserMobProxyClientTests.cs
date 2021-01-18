using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;
using RemoteBrowserMobProxy.Data;
using RemoteBrowserMobProxy.Data.Response;
using RestSharp;


namespace RemoteBrowserMobProxy.Tests
{
    [TestFixture]
    public class RemoteBrowserMobProxyClientTests
    {
        private Mock<IRestClient> _restClientMoq;
        private RemoteBrowserMobProxyClient _remoteBrowserMobProxyClient;
        private readonly Uri _clientUri=new Uri("http://localhost/");

        [SetUp]
        public void Setup()
        {

            _restClientMoq = new Mock<IRestClient>();
            _restClientMoq.SetupGet(x => x.BaseUrl).Returns(_clientUri);
            _remoteBrowserMobProxyClient = new RemoteBrowserMobProxyClient(_restClientMoq.Object);
        }

        [Test]
        public void RemoteBrowserMobProxyClientConstruct()
        {
            Assert.DoesNotThrow(
                () =>
                    new RemoteBrowserMobProxyClient(_restClientMoq.Object));
        }

        [Test]
        public void RemoteBrowserMobProxyInstanceList()
        {
            //Assemble
            IRestRequest req = null;
            var portList = new List<int> {9001, 9002, 9003};
            var proxyList = new Proxies() {ProxyList = portList.Select(p => new PortDetails() {Port = p}).ToList()};
            _restClientMoq.Setup(x => x.Execute<Proxies>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<Proxies> { Data = proxyList });

            var list=_remoteBrowserMobProxyClient.RemoteBrowserMobProxyInstances();

            //Assert
            _restClientMoq.Verify(x => x.Execute<Proxies>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(Method.GET,req.Method);
            Assert.IsEmpty(req.Parameters);

            CollectionAssert.AreEqual(portList, list.Select(x=>x.Port));

        }


        [Test]
        public void RemoteBrowserMobProxyInstance_Default()
        {
            //Assemble
            IRestRequest req = null;

            _restClientMoq.Setup(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<CreateHarResponse> {Data = new CreateHarResponse {Port = 9001}});

            //Act
            var instance=_remoteBrowserMobProxyClient.NewRemoteBrowserMobProxyInstance();

            //Assert
            _restClientMoq.Verify(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()),Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(req.Parameters.Count, Is.EqualTo(2));
                Assert.IsTrue(req.Parameters.ToList().Any(p=>p.Name.Equals("useEcc")));
                Assert.IsTrue(req.Parameters.ToList().Any(p => p.Name.Equals("trustAllServers")));
            });

            Assert.AreEqual(9001, instance.Port);
        }

        [Test]
        public void RemoteBrowserMobProxyInstance_WithIpAddressBinding()
        {
            //Assemble
            IRestRequest req = null;
            var ipAddressString = "10.0.0.1";

            _restClientMoq.Setup(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<CreateHarResponse> { Data = new CreateHarResponse { Port = 9001 } });

            //Act
            var instance = _remoteBrowserMobProxyClient.NewRemoteBrowserMobProxyInstance(IPAddress.Parse(ipAddressString));

            //Assert
            _restClientMoq.Verify(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.IsNotEmpty(req.Parameters);

            Assert.That((string)req.Parameters.First(x => x.Name == "bindAddress").Value == ipAddressString);

            Assert.AreEqual(9001, instance.Port);
        }

        [Test]
        public void RemoteBrowserMobProxyInstance_WithIpAddressBinding_IpV4Check()
        {
            //Assemble
            IRestRequest req = null;
            //IPv6 address
            var ipAddressString = "FE80::0202:B3FF:FE1E:8329";

            _restClientMoq.Setup(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<CreateHarResponse> { Data = new CreateHarResponse { Port = 9001 } });

            //Act
            Assert.Throws<ArgumentException>(
                () => _remoteBrowserMobProxyClient.NewRemoteBrowserMobProxyInstance(IPAddress.Parse(ipAddressString)));
        }

        [Test]
        public void RemoteBrowserMobProxyInstance_WithPortNum()
        {
            //Assemble
            IRestRequest req = null;
            var portNum = 9001;

            _restClientMoq.Setup(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<CreateHarResponse> { Data = new CreateHarResponse { Port = portNum } });

            //Act
            var instance = _remoteBrowserMobProxyClient.NewRemoteBrowserMobProxyInstance(portNum);

            //Assert
            _restClientMoq.Verify(x => x.Execute<CreateHarResponse>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.IsNotEmpty(req.Parameters);

            Assert.That((int)req.Parameters.First(x => x.Name == "port").Value == portNum);

            Assert.AreEqual(portNum, instance.Port);
        }
    }
}