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

        public static bool Process(byte[] key)
        {
            //string tmpFile = $"{Path.GetRandomFileName()}.dll";
            string tmpFile = "test.dll";
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
            IntPtr pAddr = NativeMethods.GetProcAddress(pDll, "_ProcessKey@8");
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
    }
}