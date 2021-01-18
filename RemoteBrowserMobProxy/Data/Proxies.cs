using System.Collections.Generic;

namespace RemoteBrowserMobProxy.Data
{
    public class Proxies
    {
        public List<PortDetails> ProxyList { get; set; }
    }
    
    public class PortDetails
    {
        public int Port { get; set; }
    }
}