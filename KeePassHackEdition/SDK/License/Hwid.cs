using Microsoft.Win32;
using System.Text;
using KeePassHackEdition.SDK.Crypto;

namespace KeePassHackEdition.SDK.License
{
    internal static class Hwid
    {
        public static string GetHwid()
        {
            string primaryKey = @"HARDWARE\DESCRIPTION\System\CentralProcessor";
            var key = Registry.LocalMachine.OpenSubKey(primaryKey);
            if (key == null)
                return string.Empty;

            string cpuids = "";
            foreach (var subKeys in key.GetSubKeyNames())
            {
                key = Registry.LocalMachine.OpenSubKey($@"{primaryKey}\{subKeys}");
                if(key == null)
                    continue;

                cpuids += key.GetValue("ProcessorNameString");
                break;
            }
            return cpuids;
        }

        public static string GetUserOs()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (key != null)
            {
                var value = key.GetValue("ProductName");
                return value.ToString();
            }

            return string.Empty;
        }

        public static string GetBiosVersion()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Hardware\Description\System");
            if(key != null)
            {
                var value = key.GetValue("BIOSVersion");
                return value.ToString();
            }
            
            return string.Empty;
        }

        public static string GetSign() => SimpleTools.Sha256(Encoding.UTF8.GetBytes($"KEEPASS_HACKER_EDITION.{GetHwid()}.{GetUserOs()}.{GetBiosVersion()}.KEEPASS_HACKER_EDITION"));
    }
}