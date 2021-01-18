using System;
using System.Collections.Generic;
using HarSharp;
using RemoteBrowserMobProxy.Data;
using RestSharp;

namespace RemoteBrowserMobProxy
{
    public interface IRemoteBrowserMobProxyInstance : IDisposable
    {
        int Port { get; }
        string CreateHar();
        string CreateHar(HarOptions options);
        void StartNewPageInHar(HarPageOptions options);
        string GetHarContentString();
        Har GetHarContent();
        void SetWhiteList(WhiteList whiteList);
        WhiteList GetWhiteList();
        void ClearWhiteList();
        void GetBlackListUrl();
        void SetBlackListUrl(BlackList blackList);
        void ClearBlackListUrl();
        void LimitBandwidth(BandwidthLimits bandwidthLimits);
        void GetBandwidthLimits();
        void SetHeaders(Dictionary<string, string> headers);
        void OverrideDnsLookups(Dictionary<string, IpEntry> dnsEntries);
        void SetBasicAuth(List<UserAccount> users);
        void SetWaitTimeouts(WaitTimeouts waitTimeouts);
        void SetProxyTimeouts(ProxyTimeouts proxyTimeouts);
        void AddUrlReWrite(UrlReWrite urlReWrite);
        void ClearUrlReWrites();
        void SetRetryCount(int count);
        void ClearDnsCache();
    }

    public class RemoteBrowserMobProxyInstance: IRemoteBrowserMobProxyInstance
    {
        private const string HarHandeler = "har";
        private const string WhiteListHandler = "whitelist";
        private const string BlackListHandler = "blacklist";
        private const string LimitHandler = "limit";
        private const string ReWriteHandler = "rewrite";

        private readonly IRestClient _restClient;

        public int Port { get; private set; }

        public RemoteBrowserMobProxyInstance(IRestClient restClient, int port)
        {
            _restClient = restClient;
            Port = port;    
        }

        public RemoteBrowserMobProxyInstance(Uri remoteBrowserMobProxyInstanceUri,int port):this(new RestClient(remoteBrowserMobProxyInstanceUri),port)
        {

        }

        public string CreateHar()
        {
            return CreateHar(new HarOptions());
        }

        public string CreateHar(HarOptions options)
        {
            var req = new RestRequest(HarHandeler, Method.PUT);

            req.AddObject(options);
            var res = _restClient.Execute(req);

            return res.Content;
        }

        public void StartNewPageInHar(HarPageOptions options)
        {
            var req = new RestRequest(HarHandeler + "/pageRef", Method.PUT);

            req.AddObject(options);
            _restClient.Execute(req);
        }

        public string GetHarContentString()
        {
            var req = new RestRequest(HarHandeler, Method.GET);

            var res = _restClient.Execute(req);
            return res.Content;
        }

        public Har GetHarContent()
        {
            var harString = GetHarContentString();
            return HarConvert.Deserialize(harString);
        }

        public void SetWhiteList(WhiteList whiteList)
        {
            var req = new RestRequest(WhiteListHandler, Method.PUT);

            req.AddObject(whiteList);

            _restClient.Execute(req);
        }

        public WhiteList GetWhiteList()
        {
            var req = new RestRequest(WhiteListHandler, Method.GET);

            var res = _restClient.Execute<WhiteList>(req);

            return res.Data;
        }

        public void ClearWhiteList()
        {
            var req = new RestRequest(WhiteListHandler, Method.DELETE);

            _restClient.Execute(req);
        }

        public void GetBlackListUrl()
        {
            var req = new RestRequest(BlackListHandler, Method.GET);
            _restClient.Execute(req);
        }

        public void SetBlackListUrl(BlackList blackList)
        {
            var req = new RestRequest(BlackListHandler, Method.PUT);

            _restClient.Execute(req);
        }

        public void ClearBlackListUrl()
        {
            var req = new RestRequest(BlackListHandler, Method.DELETE);
            _restClient.Execute(req);
        }

        public void LimitBandwidth(BandwidthLimits bandwidthLimits)
        {
            var res = new RestRequest(LimitHandler, Method.POST);
            _restClient.Execute(res);
        }

        public void GetBandwidthLimits()
        {
            var res = new RestRequest(LimitHandler, Method.GET);
            _restClient.Execute(res);
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            var res = new RestRequest("headers", Method.POST);
            res.AddJsonBody(headers);

            _restClient.Execute(res);
        }

        public void OverrideDnsLookups(Dictionary<string, IpEntry> dnsEntries)
        {
            var req = new RestRequest("hosts", Method.POST);
            req.AddJsonBody(dnsEntries);

            _restClient.Execute(req);
        }

        public void SetBasicAuth(List<UserAccount> users)
        {
            var req = new RestRequest("auth/basic", Method.POST);
            req.AddJsonBody(users);

            _restClient.Execute(req);
        }

        public void SetWaitTimeouts(WaitTimeouts waitTimeouts)
        {
            var req = new RestRequest("wait", Method.PUT);
            req.AddObject(waitTimeouts);

            _restClient.Execute(req);
        }

        public void SetProxyTimeouts(ProxyTimeouts proxyTimeouts)
        {
            var req = new RestRequest("timeout", Method.PUT);
            req.AddObject(proxyTimeouts);
            _restClient.Execute(req);
        }

        public void AddUrlReWrite(UrlReWrite urlReWrite)
        {
            var req = new RestRequest(ReWriteHandler, Method.PUT);
            req.AddObject(urlReWrite);
            _restClient.Execute(req);
        }

        public void ClearUrlReWrites()
        {
            var req = new RestRequest(ReWriteHandler, Method.DELETE);
            _restClient.Execute(req);
        }

        public void SetRetryCount(int count)
        {
            var req = new RestRequest("retry", Method.PUT);
            req.AddParameter("retrycount", count);
            _restClient.Execute(req);
        }

        public void ClearDnsCache()
        {
            var req = new RestRequest("dns/cache", Method.DELETE);
            _restClient.Execute(req);
        }

        private void ShutdownProxyInstance()
        {
            var req = new RestRequest(Method.DELETE);
            _restClient.Execute(req);
        }

        public void Dispose()
        {
            ShutdownProxyInstance();
        }
    }
}