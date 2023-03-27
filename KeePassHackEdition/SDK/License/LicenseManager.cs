using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using KeePassHackEdition.SDK.Crypto;

namespace KeePassHackEdition.SDK.License
{
    public class LicenseManager
    {
        public const string LicenseDecryptKey = "flag{omg_y0u_c4n_u53_s34rch!!!}";
        private const ulong LicenseVersion = 0x2F0FCFEDC5C89334;
        private const ulong LicenseHeader = 0x1337FF;

        private string _path;

        private byte[] _license;
        private LicenseKey _key;

        public LicenseManager(string licensePath)
        {
            _path = licensePath;
        }

        public void LoadLicense()
        {
            using (FileStream fs = new FileStream(_path, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    _license = br.ReadBytes((int)fs.Length);
                }
            }

            SecretAlg alg = new SecretAlg(LicenseDecryptKey);
            alg.Crypt(_license);
            ParseLicense();
        }

        public void ValidateLicense()
        {
            if (_key.Header != LicenseHeader)
                throw new Exception("License header error");

            if (_key.Version != LicenseVersion)
                throw new Exception("License version error");

            if (_key.Crc != GetLicenseCrc(_key))
                throw new Exception("License hash error");

            if (_key.ExpireAt < DateTime.Now.TimeOfDay.TotalSeconds)
                throw new Exception("License expired");

            if (Encoding.ASCII.GetString(_key.PcId) != Hwid.GetSign(_key.UserName))
                throw new Exception("Invalid license user");

            byte keyPreparedByte = 0;
            foreach (char c in _key.UserName)
                keyPreparedByte += (byte)((c % 2) == 0 ? (byte)c ^ 0x17 : (byte)c ^ 0x9);

            for (int i = 0; i < Usca.KeyPreparedBytes.Length; i++)
                Usca.KeyPreparedBytes[i] ^= keyPreparedByte;

        }

        public void GenerateValidLicense()
        {
#if DEBUG
            LicenseKey validKey = new LicenseKey()
            {
                Header = LicenseHeader,
                Version = LicenseVersion,
                ExpireAt = (ulong)DateTime.Now.TimeOfDay.TotalSeconds + 60 * 60 * 24 * 10,
            };

            string validName = "manager_sanya";
            validKey.UserNameSize = validName.Length;
            validKey.UserName = validName;
            validKey.PcId = Encoding.ASCII.GetBytes(Hwid.GetSign(validKey.UserName));
            validKey.Crc = GetLicenseCrc(validKey);
            byte[] validBytes = new byte[Marshal.SizeOf(validKey) + validKey.UserNameSize + validKey.PcId.Length];

            using (MemoryStream ms = new MemoryStream(validBytes))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(BitConverter.GetBytes(validKey.Header));
                    bw.Write(BitConverter.GetBytes(validKey.Version));
                    bw.Write(BitConverter.GetBytes(validKey.Crc));
                    bw.Write(BitConverter.GetBytes(validKey.ExpireAt));
                    bw.Write(validKey.PcId);
                    bw.Write(BitConverter.GetBytes(validKey.UserNameSize));
                    bw.Write(Encoding.ASCII.GetBytes(validKey.UserName));
                }
            }
            SecretAlg alg = new SecretAlg(LicenseDecryptKey);
            alg.Crypt(validBytes);
            using (FileStream fs = new FileStream("test.kpdblic", FileMode.Create))
            {
                fs.Write(validBytes, 0, validBytes.Length);
            }
#else
            throw new Exception("Nice try xDDD");
#endif
        }

        private void ParseLicense()
        {
            _key = new LicenseKey();
            using (MemoryStream ms = new MemoryStream(_license))
            {
                
                using (BinaryReader br = new BinaryReader(ms))
                {
                    _key.Header = br.ReadUInt64();
                    _key.Version = br.ReadUInt64();
                    _key.Crc = br.ReadUInt64();
                    _key.ExpireAt = br.ReadUInt64();
                    _key.PcId = br.ReadBytes(64);
                    _key.UserNameSize = br.ReadInt32();
                    _key.UserName = Encoding.ASCII.GetString(br.ReadBytes(_key.UserNameSize));
                }
            }
        }

        private ulong GetLicenseCrc(LicenseKey key)
        {
            ulong result = key.Header;
            result ^= key.Version;
            result &= key.ExpireAt;

            foreach(byte b in key.PcId)
                result += b;
            
            foreach(char c in key.UserName)
                result += c;

            return result;
        }
    }
}