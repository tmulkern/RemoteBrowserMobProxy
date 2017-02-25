using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;
using RemoteBrowserMobProxy.Data;
using RestSharp;

namespace RemoteBrowserMobProxy.Tests.UnitTests
{
    [TestFixture]
    [Category("UnitTests")]
    public class WhitelistTests
    {

        private Mock<IRestClient> _restClientMoq;
        private IRemoteBrowserMobProxyInstance _remoteBrowserMobProxyInstance;

        [SetUp]
        public void Setup()
        {

            _restClientMoq = new Mock<IRestClient>();

            var clientMoq = new Mock<IRemoteBrowserMobProxyClient>();
            clientMoq.Setup(x => x.NewRemoteBrowserMobProxyInstance())
                .Returns(new RemoteBrowserMobProxyInstance(_restClientMoq.Object, 9999));

            _remoteBrowserMobProxyInstance = clientMoq.Object.NewRemoteBrowserMobProxyInstance();
        }



        [Test]
        public void DisplayWhitelistItems()
        {
            //Assemble
            IRestRequest req = null;
            _restClientMoq.Setup(x => x.Execute<WhiteList>(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse<WhiteList>{Data = new WhiteList()});

            //Act
            var whiteList=_remoteBrowserMobProxyInstance.GetWhiteList();

            //Assert
            _restClientMoq.Verify(x => x.Execute<WhiteList>(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(Method.GET, req.Method);
            Assert.AreEqual("whitelist",req.Resource);
            Assert.IsEmpty(req.Parameters);
            Assert.IsNotNull(whiteList);
        }

        [Test]
        public void SetWhitelistItems()
        {
            //Assemble
            IRestRequest req = null;
            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse());

            //Act
            _remoteBrowserMobProxyInstance.SetWhiteList(new WhiteList());

            //Assert
            _restClientMoq.Verify(x => x.Execute(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(Method.PUT, req.Method);
            Assert.AreEqual("whitelist", req.Resource);
            Assert.IsNotEmpty(req.Parameters);


            Assert.That(req.Parameters.Exists(x=>x.Name=="regex"));
            Assert.That(req.Parameters.Exists(x => x.Name == "status"));

            Assert.That((string) req.Parameters.First(x=>x.Name=="regex").Value=="*");
            Assert.That((HttpStatusCode)req.Parameters.First(x => x.Name == "status").Value == HttpStatusCode.OK);

        }

        [Test]
        public void ClearWhitelistItems()
        {
            //Assemble
            IRestRequest req = null;
            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse());

            //Act
            _remoteBrowserMobProxyInstance.ClearWhiteList();

            //Assert
            _restClientMoq.Verify(x => x.Execute(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(Method.DELETE, req.Method);
            Assert.AreEqual("whitelist", req.Resource);
            Assert.IsEmpty(req.Parameters);      
        }
    }
}