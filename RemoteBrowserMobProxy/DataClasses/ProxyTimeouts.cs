namespace RemoteBrowserMobProxy.DataClasses
{
    public class ProxyTimeouts
    {
        public long requestTimeout { get; set; }
        public long readTimeout { get; set; }
        public long connectionTimeout { get; set; }
        public long dnsCacheTimeout { get; set; }
    }
}