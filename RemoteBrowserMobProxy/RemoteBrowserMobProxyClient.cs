using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Flurl;
using RemoteBrowserMobProxy.DataClasses.Response;
using RestSharp;

namespace RemoteBrowserMobProxy
{
    public class RemoteBrowserMobProxyClient
    {
        private IRestClient _restClient;

        public RemoteBrowserMobProxyClient(Uri browserMobProxyRemoteUri)
        {
            _restClient=new RestClient(browserMobProxyRemoteUri);
            
        }

        public IReadOnlyCollection<RemoteBrowserMobProxyInstance> RemoteBrowserMobProxyInstances()
        {
            var req = new RestRequest(Method.GET);

            var res=_restClient.Execute<List<int>>(req);

            return
                res.Data.Select(
                    portNum =>
                        new RemoteBrowserMobProxyInstance(new Uri(_restClient.BaseUrl,
                            new Uri(portNum.ToString())),portNum)).ToList().AsReadOnly();
        }

        public RemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(IPAddress ip4BindingAddress=null, int port = 0)
        {
            var req = new RestRequest(Method.POST);

            if (port != 0)
            {
                req.AddParameter("port", port);
            }

            if (ip4BindingAddress != null)
            {
                if (ip4BindingAddress.AddressFamily != AddressFamily.InterNetwork)
                    throw new ArgumentException("Bining address must be IPv4");

                req.AddParameter("bindAddress", ip4BindingAddress.ToString());
            }

            var res = _restClient.Execute<CreateHarResponse>(req);

            var url=Url.Combine(_restClient.BaseUrl.ToString(), res.Data.port.ToString());

            return new RemoteBrowserMobProxyInstance(new Uri(url),res.Data.port);
        }
    }
}
