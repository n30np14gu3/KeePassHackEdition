using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace KeePassHackEdition.SDK.License
{
    internal class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        internal static extern bool FreeLibrary(IntPtr hModule);
    }

    public class LicenseTools
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate IntPtr ProcessKey(IntPtr key, uint key_size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private delegate IntPtr CryptResponse(IntPtr key, uint key_size, bool decrypt);

        public static bool Process(byte[] key)
        {
            string tmpFile = $"{Path.GetRandomFileName()}.dll";
            using (FileStream fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
            {
                fs.Write(Properties.Resources.tools, 0, Properties.Resources.tools.Length);
            }
            IntPtr pDll = NativeMethods.LoadLibrary(tmpFile);
            if (pDll == IntPtr.Zero)
            {
                File.Delete(tmpFile);
                return false;
            }
            IntPtr pAddr = NativeMethods.GetProcAddress(pDll, "_ProcessPreparedBytes@8");
            if (pAddr == IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(pDll);
                File.Delete(tmpFile);
                return false;
            }
            ProcessKey process = (ProcessKey)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(ProcessKey));

            IntPtr bytes = Marshal.AllocHGlobal(key.Length);
            Marshal.Copy(key, 0, bytes, key.Length);
            IntPtr bytesResult = process(bytes, (uint)key.Length);
            bool result = false;
            if (bytesResult != IntPtr.Zero)
            {
                Marshal.Copy(bytesResult, key, 0, key.Length);
                result = true;
            }
            Marshal.FreeHGlobal(bytes);
            NativeMethods.FreeLibrary(pDll);
            File.Delete(tmpFile);
            return result;
        }

        public static bool DecryptResponse(byte[] response)
        {
            string tmpFile = $"{Path.GetRandomFileName()}.dll";
            using (FileStream fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
            {
                fs.Write(Properties.Resources.tools, 0, Properties.Resources.tools.Length);
            }
            IntPtr pDll = NativeMethods.LoadLibrary(tmpFile);
            if (pDll == IntPtr.Zero)
            {
                File.Delete(tmpFile);
                return false;
            }
            IntPtr pAddr = NativeMethods.GetProcAddress(pDll, "_CryptResponse@12");
            if (pAddr == IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(pDll);
                File.Delete(tmpFile);
                return false;
            }
            CryptResponse process = (CryptResponse)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(CryptResponse));

            IntPtr bytes = Marshal.AllocHGlobal(response.Length);
            Marshal.Copy(response, 0, bytes, response.Length);
            IntPtr bytesResult = process(bytes, (uint)response.Length, true);
            bool result = false;
            if (bytesResult != IntPtr.Zero)
            {
                Marshal.Copy(bytesResult, response, 0, response.Length);
                result = true;
            }
            Marshal.FreeHGlobal(bytes);
            NativeMethods.FreeLibrary(pDll);
            File.Delete(tmpFile);
            return result;
        }

        public static bool EncryptResponse(byte[] response)
        {
#if DEBUG
            string tmpFile = $"{Path.GetRandomFileName()}.dll";
            using (FileStream fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
            {
                fs.Write(Properties.Resources.tools, 0, Properties.Resources.tools.Length);
            }
            IntPtr pDll = NativeMethods.LoadLibrary(tmpFile);
            if (pDll == IntPtr.Zero)
            {
                File.Delete(tmpFile);
                return false;
            }
            IntPtr pAddr = NativeMethods.GetProcAddress(pDll, "_CryptResponse@12");
            if (pAddr == IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(pDll);
                File.Delete(tmpFile);
                return false;
            }
            CryptResponse process = (CryptResponse)Marshal.GetDelegateForFunctionPointer(pAddr, typeof(CryptResponse));

            IntPtr bytes = Marshal.AllocHGlobal(response.Length);
            Marshal.Copy(response, 0, bytes, response.Length);
            IntPtr bytesResult = process(bytes, (uint)response.Length, false);
            bool result = false;
            if (bytesResult != IntPtr.Zero)
            {
                Marshal.Copy(bytesResult, response, 0, response.Length);
                result = true;
            }
            Marshal.FreeHGlobal(bytes);
            NativeMethods.FreeLibrary(pDll);
            File.Delete(tmpFile);
            return result;
#endif
            throw new NotImplementedException("Nice try XD");
        }
    }
}