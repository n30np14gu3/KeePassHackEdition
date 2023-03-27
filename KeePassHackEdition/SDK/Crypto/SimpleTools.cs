using System;
using System.Linq;
using System.Security.Cryptography;

namespace KeePassHackEdition.SDK.Crypto
{
    public class SimpleTools
    {
        public static string Sha256(byte[] data) => BitConverter
            .ToString(new SHA256CryptoServiceProvider().ComputeHash(data)).Replace("-", string.Empty);

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

    }
}