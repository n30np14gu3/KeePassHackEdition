﻿using System;
using System.Security.Cryptography;
using System.IO;

namespace KeePassHackEdition.SDK.Crypto
{
    public class Crc32 : HashAlgorithm
    {
        public const uint DefaultPolynomial = 0xedb88320;
        public const uint DefaultSeed = 0xffffffff;

        private uint _hash;
        private readonly uint _seed;
        private readonly uint[] _table;
        private static uint[] _defaultTable;

        public Crc32()
        {
            _table = InitializeTable(DefaultPolynomial);
            _seed = DefaultSeed;
            Initialize();
        }

        public Crc32(uint polynomial, uint seed)
        {
            _table = InitializeTable(polynomial);
            this._seed = seed;
            Initialize();
        }

        public sealed override void Initialize()
        {
            _hash = _seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            _hash = CalculateHash(_table, _hash, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            byte[] hashBuffer = BitConverter.GetBytes(~_hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize
        {
            get { return 32; }
        }

        public static uint Compute(byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), DefaultSeed, buffer, 0, buffer.Length);
        }

        public static uint Compute(uint seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(DefaultPolynomial), seed, buffer, 0, buffer.Length);
        }

        public static uint Compute(uint polynomial, uint seed, byte[] buffer)
        {
            return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        private static uint[] InitializeTable(uint polynomial)
        {
            if (polynomial == DefaultPolynomial && _defaultTable != null)
                return _defaultTable;

            uint[] createTable = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                uint entry = (uint)i;
                for (int j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry >>= 1;
                createTable[i] = entry;
            }

            if (polynomial == DefaultPolynomial)
                _defaultTable = createTable;

            return createTable;
        }

        private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, int start, int size)
        {
            uint crc = seed;
            for (int i = start; i < size; i++)
                unchecked
                {
                    crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                }
            return crc;
        }

        public string Get(string FilePath)
        {
            Crc32 crc32 = new Crc32();
            String hash = String.Empty;

            using (FileStream fs = File.Open(FilePath, FileMode.Open))
                foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

            return hash;
        }
    }
}