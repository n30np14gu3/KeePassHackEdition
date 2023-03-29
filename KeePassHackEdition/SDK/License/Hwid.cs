using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using KeePassHackEdition.SDK.Crypto;

namespace KeePassHackEdition.SDK.License
{
    internal static class Hwid
    {
        public static string GetHwid() =>
            (from x in new ManagementObjectSearcher("SELECT * FROM Win32_processor").Get().OfType<ManagementObject>()
                select x.GetPropertyValue("ProcessorId")).First().ToString();

        public static string GetUserOs() =>
            (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                select x.GetPropertyValue("Caption")).First().ToString();

        public static string GetBiosVersion() =>
            (from x in new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS").Get().OfType<ManagementObject>()
                select x.GetPropertyValue("SerialNumber")).First().ToString();

        public static string GetSign() => SimpleTools.Sha256(Encoding.UTF8.GetBytes($"KEEPASS_HACKER_EDITION.{GetHwid()}.{GetUserOs()}.{GetBiosVersion()}.KEEPASS_HACKER_EDITION"));
    }
}