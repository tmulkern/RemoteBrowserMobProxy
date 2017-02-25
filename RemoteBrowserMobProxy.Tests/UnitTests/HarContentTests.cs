using System.Linq;
using Moq;
using NUnit.Framework;
using RemoteBrowserMobProxy.Data;
using RestSharp;

namespace RemoteBrowserMobProxy.Tests.UnitTests
{
    [TestFixture]
    [Category("UnitTests")]
    public class HarContentTests
    {

        #region HarSampleContent
            private const string HarStringSample = @"{""log"":{""version"":""1.2"",""creator"":{""name"":""BrowserMob Proxy"",""version"":""2.1.0-beta-4-littleproxy"",""comment"":""""},""pages"":[{""id"":""Page 0"",""startedDateTime"":""2016-02-22T17:11:35.759Z"",""title"":""Page 0"",""pageTimings"":{""comment"":""""},""comment"":""""}],""entries"":[{""pageref"":""Page 0"",""startedDateTime"":""2016-02-22T17:11:35.762Z"",""request"":{""method"":""GET"",""url"":""http://www.google.ie/"",""httpVersion"":""HTTP/1.1"",""cookies"":[],""headers"":[],""queryString"":[],""headersSize"":91,""bodySize"":0,""comment"":""""},""response"":{""status"":200,""statusText"":""OK"",""httpVersion"":""HTTP/1.1"",""cookies"":[{""name"":""NID"",""value"":""76=2IRU9rq_l1Z2ijDRT8Fhni-ym7N_3GwXfNJNVRwsv7lMufiGgSvscvQuVEJQgfQ8-h1E_obdyKNBTwGzbyS7eB09s-oZn8nY7mB3rSiw3kRCKyn3WCMhDPxRQef2TydD"",""path"":""/"",""domain"":"".google.ie"",""expires"":""2016-02-22T21:35:07.109Z"",""httpOnly"":true,""secure"":false}],""headers"":[],""content"":{""size"":0,""mimeType"":""text/html; charset=ISO-8859-1"",""comment"":""""},""redirectURL"":"""",""headersSize"":639,""bodySize"":54147,""comment"":""""},""cache"":{},""timings"":{""comment"":"""",""send"":0,""connect"":17,""dns"":2,""ssl"":-1,""blocked"":0,""receive"":3,""wait"":146},""serverIPAddress"":""216.58.198.67"",""comment"":"""",""time"":170}],""comment"":""""}}";
        #endregion



        private Mock<IRestClient> _restClientMoq;
        private IRemoteBrowserMobProxyInstance _remoteBrowserMobProxyInstance;

        [SetUp]
        public void Setup()
        {

            _restClientMoq = new Mock<IRestClient>();

            var clientMoq = new Mock<IRemoteBrowserMobProxyClient>();

            clientMoq.Setup(x => x.NewRemoteBrowserMobProxyInstance())
                .Returns(new RemoteBrowserMobProxyInstance(_restClientMoq.Object,9999));
            
            _remoteBrowserMobProxyInstance = clientMoq.Object.NewRemoteBrowserMobProxyInstance();
        }

        [Test]
        public void CreateHar()
        {
            //Assemble
            IRestRequest req=null;

            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request =>req = request)
                .Returns(new RestResponse());

            //Act
            _remoteBrowserMobProxyInstance.CreateHar();


            //Assert
            _restClientMoq.Verify(x=>x.Execute(It.IsAny<IRestRequest>()),Times.Once);

            Assert.AreEqual(Method.PUT,req.Method);
            Assert.AreEqual(req.Resource, "har");
            Assert.IsNotEmpty(req.Parameters);

            var parameterList = req.Parameters.Select(x => new { x.Name, Value = (bool)x.Value }).ToList();

            CollectionAssert.Contains(parameterList, new { Name = "captureHeaders", Value = false });
            CollectionAssert.Contains(parameterList, new { Name = "captureContent", Value = false });
            CollectionAssert.Contains(parameterList, new { Name = "captureBinaryContent", Value = false });
   
        }

        [Test]
        public void StartNewPageInHar()
        {
            //Assemble
            IRestRequest req = null;

            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse());

            //Act
            _remoteBrowserMobProxyInstance.StartNewPageInHar(new HarPageOptions());


            //Assert
            _restClientMoq.Verify(x => x.Execute(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(Method.PUT, req.Method);
            Assert.AreEqual(req.Resource, "har/pageRef");
            Assert.IsEmpty(req.Parameters);
            
        }

        [Test]
        public void GetHarContentString()
        {
            //Assemble
            IRestRequest req = null;

            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse { Content = HarStringSample });

            //Act
            var har=_remoteBrowserMobProxyInstance.GetHarContentString();

            //Assert
            _restClientMoq.Verify(x => x.Execute(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual(HarStringSample,har);
            Assert.AreEqual(Method.GET, req.Method);
            Assert.AreEqual(req.Resource, "har");
            Assert.IsEmpty(req.Parameters);    
        }

        [Test]
        public void GetHarContent()
        {
            //Assemble
            IRestRequest req = null;

            _restClientMoq.Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Callback<IRestRequest>(request => req = request)
                .Returns(new RestResponse{Content = HarStringSample});

            //Act
            var har=_remoteBrowserMobProxyInstance.GetHarContent();


            //Assert
            _restClientMoq.Verify(x => x.Execute(It.IsAny<IRestRequest>()), Times.Once);

            Assert.AreEqual("http://www.google.ie/",har.Log.Entries[0].Request.Url.ToString());
            Assert.AreEqual(Method.GET, req.Method);
            Assert.AreEqual(req.Resource, "har");
            Assert.IsEmpty(req.Parameters);   
    
        }
    }
}