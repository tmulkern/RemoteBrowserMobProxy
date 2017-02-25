using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RemoteBrowserMobProxy.Data
{
    public class WhiteList
    {
        public WhiteList()
        {
            RegexList=new List<string>();
            status = HttpStatusCode.OK;
        }

        public string regex
        {
            get { return RegexList.Any()?string.Join(",", RegexList):"*"; }
        }

        public HttpStatusCode status { get; set; }

        private List<string> RegexList { get; set; }
  
        public void AddUrlRegex(Regex regexPattern)
        {
            RegexList.Add(regexPattern.ToString());   
        }
    }
}