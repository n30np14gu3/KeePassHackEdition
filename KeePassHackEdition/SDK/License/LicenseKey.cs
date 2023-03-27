using System.Runtime.InteropServices;

namespace KeePassHackEdition.SDK.License
{
    public struct LicenseKey
    {
        public ulong Header;
        public ulong Version;
        public ulong Crc;
        public ulong ExpireAt;
        public byte[] PcId;
        public int UserNameSize;
        public string UserName;
    }
}