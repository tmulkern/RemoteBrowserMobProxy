using System.Net;
using System.Text.RegularExpressions;

namespace RemoteBrowserMobProxy.Data
{
    public class BlackList
    {
        public Regex regex { get; set; }
        public HttpStatusCode status { get; set; }
        public Regex method { get; set; }
    }
}