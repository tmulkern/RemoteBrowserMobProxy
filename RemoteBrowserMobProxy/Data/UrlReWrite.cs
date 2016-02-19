using System;
using System.Text.RegularExpressions;

namespace RemoteBrowserMobProxy.Data
{
    public class UrlReWrite
    {
        public Regex matchRegex { get; set; }
        public Uri replace { get; set; }
    }
}