namespace RemoteBrowserMobProxy.Data
{
    public class HarOptions : HarPageOptions
    {
        public bool captureHeaders { get; private set; }
        public bool captureContent { get; private set; }
        public bool captureBinaryContent { get; private set; }
    }
}