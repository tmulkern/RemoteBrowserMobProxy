namespace RemoteBrowserMobProxy.Data
{
    public class BandwidthLimits
    {
        public int downstreamKbps { get; set; }
        public int upstreamKbps { get; set; }
        public int downstreamMaxKB { get; set; }
        public int upstreamMaxKB { get; set; }
        public int latency { get; set; }
        public bool enable { get; set; }
        public decimal payloadPercentage { get; set; }
        public long maxBitsPerSecond { get; set; }
    }
}