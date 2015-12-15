using System;
using System.Collections.Generic;
using RemoteBrowserMobProxy.DataClasses;
using RestSharp;

namespace RemoteBrowserMobProxy
{
    public class RemoteBrowserMobProxyInstance:IDisposable
    {
        private readonly RestClient _restClient;

        public int Port { get; private set; }

        public RemoteBrowserMobProxyInstance(Uri remoteBrowserMobProxyInstanceUri,int port)
        {
            _restClient=new RestClient(remoteBrowserMobProxyInstanceUri);
            Port = port;
        }

        public string CreateHar(HarOptions options)
        {
            var req = new RestRequest("har", Method.PUT);

            req.AddObject(options);
            var res = _restClient.Execute(req);

            return res.Content;
        }

        public void StartNewPageInHar(HarPageOptions options)
        {
            var req = new RestRequest("har", Method.PUT);

            req.AddObject(options);
            var res = _restClient.Execute(req);
        }

        public string GetHarContent(HarOptions options)
        {
            var req = new RestRequest("har", Method.GET);

            req.AddObject(options);
            var res = _restClient.Execute(req);
            return res.Content;
        }

        public void SetWhiteList(WhiteList whiteList)
        {
            var req = new RestRequest("whiteList",Method.PUT);

            req.AddObject(whiteList);

            _restClient.Execute(req);
        }

        public WhiteList GetWhiteList()
        {
            var req = new RestRequest("whiteList",Method.GET);

            var res=_restClient.Execute <WhiteList>(req);

            return res.Data;
        }

        public void ClearWhiteList()
        {
            var req = new RestRequest("whiteList", Method.DELETE);

            _restClient.Execute(req);
        }

        public void GetBlackListUrl()
        {
            var req = new RestRequest("blacklist", Method.GET);
            _restClient.Execute(req);
        }

        public void SetBlackListUrl(BlackList blackList)
        {
            var req = new RestRequest("blacklist", Method.PUT);

            _restClient.Execute(req);
        }

        public void ClearBlackListUrl()
        {
            var req = new RestRequest("blacklist", Method.DELETE);
            _restClient.Execute(req);
        }

        public void LimitBandwidth(BandwidthLimits bandwidthLimits)
        {
            var res = new RestRequest("limit", Method.POST);
            _restClient.Execute(res);
        }

        public void GetBandwidthLimits()
        {
            var res = new RestRequest("limit", Method.GET);
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
            var req = new RestRequest("rewrite", Method.PUT);
            req.AddObject(urlReWrite);
            _restClient.Execute(req);
        }

        public void ClearUrlReWrites()
        {
            var req = new RestRequest("rewrite", Method.DELETE);
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
            var res = _restClient.Execute(req);
        }

        public void Dispose()
        {
            ShutdownProxyInstance();
        }
    }
}