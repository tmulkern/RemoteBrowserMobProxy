using System;

namespace RemoteBrowserMobProxy.Data
{
    public class IpEntry
    {
        private readonly string _ipAddress;

        public IpEntry(byte one, byte two, byte three, byte four)
        {
            if (one == 0)
            {
                throw new ArgumentException("First Byte Cannot be 0");
            }

            _ipAddress = String.Format("{0}.{1}.{2}.{3}", one, two, three, four);
        }

        public override string ToString()
        {
            return _ipAddress;
        }
    }
}