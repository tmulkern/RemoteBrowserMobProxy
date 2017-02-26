using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using RemoteBrowserMobProxy.Data.Response;
using RestSharp;

namespace RemoteBrowserMobProxy
{
    public interface IRemoteBrowserMobProxyClient
    {
        IReadOnlyCollection<IRemoteBrowserMobProxyInstance> RemoteBrowserMobProxyInstances();
        IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(IPAddress ip4BindingAddress, int port);
        IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(int port);
        IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(IPAddress ip4BindingAddress);
        IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance();
    }

    public class RemoteBrowserMobProxyClient : IRemoteBrowserMobProxyClient
    {
        private readonly IRestClient _restClient;

        internal RemoteBrowserMobProxyClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public RemoteBrowserMobProxyClient(Uri browserMobProxyRemoteUri):this(new RestClient(browserMobProxyRemoteUri))
        {

        }

        public IReadOnlyCollection<IRemoteBrowserMobProxyInstance> RemoteBrowserMobProxyInstances()
        {
            var req = new RestRequest(Method.GET);

            var res=_restClient.Execute<List<int>>(req);

            return
                res.Data.Select(
                    portNum =>
                        new RemoteBrowserMobProxyInstance(new Uri(_restClient.BaseUrl,
                            new Uri(portNum.ToString(), UriKind.Relative)), portNum)).ToList().AsReadOnly();
        }

        public IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance()
        {
            return NewRemoteBrowserMobProxyInstance(null, 0);
        }

        public IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(IPAddress ip4BindingAddress)
        {
            return NewRemoteBrowserMobProxyInstance(ip4BindingAddress, 0);
        }

        public IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(int port)
        {
            return NewRemoteBrowserMobProxyInstance(null, port);
        }


        public IRemoteBrowserMobProxyInstance NewRemoteBrowserMobProxyInstance(IPAddress ip4BindingAddress, int port)
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

            var url = string.Format("{0}/{1}", _restClient.BaseUrl, res.Data.Port);

            return new RemoteBrowserMobProxyInstance(new Uri(url),res.Data.Port);
        }
    }
}
