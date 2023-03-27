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
        
        private const string PayloadHash = "94E38498E161B46710A4A69A9302DFBF59419168C171688FF8BC2D052DF59E12";
        private const string Alphabet = "abcdefghijklmnopqrtsuvwxzy1234567890+-/\\!#$%^&*()_+-=-";

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

            if (_key.ExpireAt < (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)
                throw new Exception("License expired");

            if (Encoding.ASCII.GetString(_key.PcId) != Hwid.GetSign(_key.UserName))
                throw new Exception("Invalid license user");

            CryptPayload(ref _key);
            string hash  = SimpleTools.Sha256(_key.LicensePayload);
            if (SimpleTools.Sha256(_key.LicensePayload) != PayloadHash)
                throw new Exception("Invalid payload hash");


            Usca.KeyPreparedBytes = new byte[_key.LicensePayload.Length];
            for (int i = 0; i < Usca.KeyPreparedBytes.Length; i++)
                Usca.KeyPreparedBytes[i] = (byte)(_key.LicensePayload[i] ^ 0x3E);

        }

        public void GenerateValidLicense()
        {
#if DEBUG
            LicenseKey validKey = new LicenseKey()
            {
                Header = LicenseHeader,
                Version = LicenseVersion,
                ExpireAt = (ulong)(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + 60 * 60 * 24 * 10,
            };

            string validName = "manager_sanya";
            validKey.LicensePayload = Encoding.ASCII.GetBytes("flag{n153_k3yg3n}");
            validKey.UserNameSize = validName.Length;
            validKey.UserName = validName;
            validKey.PcId = Encoding.ASCII.GetBytes(Hwid.GetSign(validKey.UserName));
            validKey.Crc = GetLicenseCrc(validKey);
            byte[] validBytes = new byte[Marshal.SizeOf(validKey) + validKey.UserNameSize + validKey.PcId.Length + validKey.LicensePayload.Length];

            CryptPayload(ref validKey);

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
                    bw.Write(validKey.LicensePayload);
                }
            }
            SecretAlg alg = new SecretAlg(LicenseDecryptKey);
            alg.Crypt(validBytes);
            using (FileStream fs = new FileStream(_path, FileMode.Create))
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
                    _key.LicensePayload = br.ReadBytes(17);
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

        private void CryptPayload(ref LicenseKey key)
        {
            int seed = 0;

            for (int i = 0; i < key.UserName.Length; i++)
                seed += key.UserName[i];

            for (int i = 0; i < key.PcId.Length; i++)
                seed += key.PcId[i];

            seed ^= (int)key.Version % int.MaxValue;
            Random rnd = new Random(seed);

            byte[] cryptKey = new byte[8];
            for (int i = 0; i < cryptKey.Length; i++)
                cryptKey[i] = (byte)Alphabet[rnd.Next(8)];

            SecretAlg algPayloadCrypt = new SecretAlg(Encoding.ASCII.GetString(cryptKey));
            algPayloadCrypt.Crypt(key.LicensePayload);
        }

        public string GenerateActivationRequest()
        {
            ActivationRequest request = new ActivationRequest
            {
                LicenseVersion = _key.Version,
                RequestId = (ulong)DateTime.Now.Ticks,
                UserId = _key.UserName
            };

            byte[] xml = Encoding.ASCII.GetBytes(Serializer<ActivationRequest>.Serialize(request));
            string cryptKey = VMProtect.SDK.DecryptString("KLkpN6a8Tj68t1HfEuDHjuK9crMZIZA8S1A8o1n2B0oE7R5DGjK");
            SecretAlg alg = new SecretAlg(cryptKey);
            alg.Crypt(xml);
            return Convert.ToBase64String(xml);
        }

        public void ValidateActivationResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                throw new Exception("Empty response");

            byte[] responseBytes = Convert.FromBase64String(response);
            string cryptKey = VMProtect.SDK.DecryptString("v93G0rSQKHGmVygLuedr4BvxUKqTiACs2Knv0whKtGodNcgnJCy");
            string sessionKeyHeader = VMProtect.SDK.DecryptString("sskey_");
            SecretAlg alg = new SecretAlg(cryptKey);
            alg.Crypt(responseBytes);

            ActivationResponse activation = Serializer<ActivationResponse>.Deserialize(Encoding.ASCII.GetString(responseBytes)) as ActivationResponse;
            if (activation == null)
                throw new Exception("Invalid response");

            if (!activation.Success)
                throw new Exception("Invalid response status");

            if (DateTime.Now.Ticks - (long)activation.ResponseId < 0)
                throw new Exception("Invalid response ID");

            if (activation.LicenseVersion != _key.Version)
                throw new Exception("Invalid response version");

            byte[] sessionKey = Convert.FromBase64String(activation.SessionKey);
            if (sessionKey.Length < sessionKeyHeader.Length)
                throw new Exception("Invalid session key");

            for(int i = 0; i < sessionKeyHeader.Length; i++)
                if (sessionKey[i] != (byte)sessionKeyHeader[i])
                    throw new Exception("Invalid session key magic");

            byte[] versionBytes = BitConverter.GetBytes(activation.LicenseVersion);
            for (int i = 0; i < versionBytes.Length; i++)
                versionBytes[i] ^= sessionKey[i % sessionKey.Length];

            Usca.KeyPreparedBytes2 = new byte[versionBytes.Length];
            for (int i = 0; i < Usca.KeyPreparedBytes2.Length; i++)
                Usca.KeyPreparedBytes2[i] = (byte)((byte)i ^ versionBytes[i]);

            if (!LicenseTools.Process(Usca.KeyPreparedBytes2))
                throw new Exception("Can't process prepared data");
        }

        public string GenerateValidActivationResponse()
        {
#if DEBUG
            string cryptKey = "v93G0rSQKHGmVygLuedr4BvxUKqTiACs2Knv0whKtGodNcgnJCy";
            ActivationResponse response = new ActivationResponse
            {
                LicenseVersion = _key.Version,
                ResponseId = 0,
                SessionKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(@"sskey_1337865")),
                Success = true
            };
            byte[] xml = Encoding.ASCII.GetBytes(Serializer<ActivationResponse>.Serialize(response));
            SecretAlg alg = new SecretAlg(cryptKey);
            alg.Crypt(xml);
            return Convert.ToBase64String(xml);
#else
            throw new Exception("Nice try xD");
#endif
        }
    }
}