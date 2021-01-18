namespace RemoteBrowserMobProxy.Data
{
    public class HarOptions : HarPageOptions
    {
        public bool captureHeaders { get; set; }
        public bool captureCookies { get; set; }
        public bool captureContent { get; set; }
        public bool captureBinaryContent { get; set; }
    }
}