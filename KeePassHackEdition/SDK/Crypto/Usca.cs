 using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KeePassHackEdition.SDK.Crypto
{
    public class Usca
    {
        private byte[] _key;
        private byte[] _iv;
        public static byte[] KeyPreparedBytes;
        public static Func<byte[], byte[]> KeyTransformFunc;


        public Usca(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;

            if (KeyPreparedBytes != null)
            {
                for (int i = 0; i < key.Length; i++)
                    _key[i] ^= KeyPreparedBytes[i % KeyPreparedBytes.Length];
            }

            if (KeyTransformFunc != null)
            {
                _key = KeyTransformFunc(_key);
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypt = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(cs))
                    {
                        bw.Write(data);
                    }
                }
                return ms.ToArray();
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            Aes aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return  Encoding.ASCII.GetBytes(sr.ReadToEnd());
                    }
                }

            }
        }

        public byte[] Key()
        {
            return _key;
        }

        public byte[] Iv()
        {
            return _iv;
        }
    }
}