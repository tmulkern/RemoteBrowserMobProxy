using System;
using System.Text.RegularExpressions;

namespace RemoteBrowserMobProxy.DataClasses
{
    public class UrlReWrite
    {
        public Regex matchRegex { get; set; }
        public Uri replace { get; set; }
    }
}