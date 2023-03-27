using System.Text;

namespace KeePassHackEdition.SDK.Crypto
{
    //RC4 Alg
    public class SecretAlg
    {
        private readonly byte[] _key;

        private readonly byte[] _secretBlock;
        private int _x;
        private int _y;

        public SecretAlg(string key)
        {
            _key = Encoding.ASCII.GetBytes(key);
            _secretBlock = new byte[256];
            _x = 0;
            _y = 0;
        }

        public void Crypt(byte[] data)
        {
            _x = 0;
            _y = 0;
            SecretInitKey();
            for (int i = 0; i < data.Length; i++)
                data[i] ^= SecretGetKeyItem();
        }

        private void SecretInitKey()
        {
            for (int i = 0; i < _secretBlock.Length; i++)
                _secretBlock[i] = (byte)i;

            int j = 0;
            for (int i = 0; i < _secretBlock.Length; i++)
            {
                //Calc index
                j = (j + _secretBlock[i] + _secretBlock[i % _key.Length]) % 256;

                //swap
                byte tmp = _secretBlock[i];
                _secretBlock[i] = _secretBlock[j];
                _secretBlock[j] = tmp;
            }
        }

        private byte SecretGetKeyItem()
        {
            _x = (_x + 1) % 256;
            _y = (_y + _secretBlock[_x]) % 256;
            byte tmp = _secretBlock[_x];
            _secretBlock[_x] = _secretBlock[_y];
            _secretBlock[_y] = tmp;

            return _secretBlock[(_secretBlock[_x] + _secretBlock[_y]) % 256];
        }
    }
}