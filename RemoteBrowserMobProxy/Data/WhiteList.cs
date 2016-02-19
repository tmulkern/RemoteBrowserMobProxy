using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace RemoteBrowserMobProxy.Data
{
    public class WhiteList
    {

        public string regex
        {
            get { return string.Join(",", _regexList); }
        }

        public HttpStatusCode status { get; set; }

        private List<string> _regexList;
  
        public void AddUrlRegex(Regex regexPattern)
        {
            _regexList.Add(regexPattern.ToString());   
        }
    }

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