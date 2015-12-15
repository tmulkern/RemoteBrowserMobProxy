namespace RemoteBrowserMobProxy.DataClasses
{
    public class HarOptions : HarPageOptions
    {
        // ReSharper disable InconsistentNaming
        public bool captureHeaders { get; set; }
        public bool captureContent { get; set; }
        public bool captureBinaryContent { get; set; }
    }
}