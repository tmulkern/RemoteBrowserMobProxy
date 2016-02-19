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

        private readonly List<string> _regexList=new List<string>();
  
        public void AddUrlRegex(Regex regexPattern)
        {
            _regexList.Add(regexPattern.ToString());   
        }
    }
}