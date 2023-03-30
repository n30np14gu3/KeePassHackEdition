#include <stdint.h>

#define bool unsigned char
#define byte unsigned char
#define true 1
#define false 0
#define XOR_KEY 0xD2
#define EXPORTED   __declspec( dllexport )
#define MEMORY_SIZE (1024 * 1024)
#define MEMORY_KEY_SIZE (1024 * 6)


#define SHA2_SHFR(x, n)    (x >> n)
#define SHA2_ROTR(x, n)   ((x >> n) | (x << ((sizeof(x) << 3) - n)))
#define SHA2_ROTL(x, n)   ((x << n) | (x >> ((sizeof(x) << 3) - n)))
#define SHA2_CH(x, y, z)  ((x & y) ^ (~x & z))
#define SHA2_MAJ(x, y, z) ((x & y) ^ (x & z) ^ (y & z))
#define SHA256_F1(x) (SHA2_ROTR(x,  2) ^ SHA2_ROTR(x, 13) ^ SHA2_ROTR(x, 22))
#define SHA256_F2(x) (SHA2_ROTR(x,  6) ^ SHA2_ROTR(x, 11) ^ SHA2_ROTR(x, 25))
#define SHA256_F3(x) (SHA2_ROTR(x,  7) ^ SHA2_ROTR(x, 18) ^ SHA2_SHFR(x,  3))
#define SHA256_F4(x) (SHA2_ROTR(x, 17) ^ SHA2_ROTR(x, 19) ^ SHA2_SHFR(x, 10))


byte OUT_MEMORY[MEMORY_SIZE];
byte CACHED_KEY[MEMORY_KEY_SIZE];
byte S_Block[MEMORY_KEY_SIZE];


uint_least32_t calc_crc32(unsigned char* buf, size_t len)
{
	uint_least32_t crc_table[256];
	uint_least32_t crc; int i, j;

	for (i = 0; i < 256; i++)
	{
		crc = i;
		for (j = 0; j < 8; j++)
			crc = crc & 1 ? (crc >> 1) ^ 0xEDB88320UL : crc >> 1;

		crc_table[i] = crc;
	};

	crc = 0xFFFFFFFFUL;

	while (len--)
		crc = crc_table[(crc ^ *buf++) & 0xFF] ^ (crc >> 8);

	return crc ^ 0xFFFFFFFFUL;
}

EXPORTED byte* __stdcall ProcessPreparedBytes(byte* key, unsigned key_size)
{
	if (key == 0 || key_size > MEMORY_KEY_SIZE)
		return false;

	for (unsigned i = 0; i < MEMORY_SIZE; i++)
		OUT_MEMORY[i] = 0;

	for (unsigned i = 0; i < MEMORY_KEY_SIZE; i++)
		CACHED_KEY[i] = 0;

	for (unsigned i = 0; i < key_size; i++)
		CACHED_KEY[i] = key[i];

	for (unsigned i = 0; i < MEMORY_KEY_SIZE; i++)
		S_Block[i] = (byte)(i % 256);

	for (int i = MEMORY_KEY_SIZE - 1; i >= 0; i--)
	{
		if (i % 2 == 0)
		{
			S_Block[i] ^= SHA256_F4(S_Block[i]);
			S_Block[i] &= SHA256_F3(S_Block[i]);
			S_Block[i] |= SHA256_F2(S_Block[i]);
			S_Block[i] ^= SHA256_F1(S_Block[i]);
			S_Block[i] |= SHA2_ROTR(S_Block[i], 3);
		}
		else
		{
			S_Block[i] ^= SHA256_F1(S_Block[i]);
			S_Block[i] |= SHA256_F2(S_Block[i]);
			S_Block[i] &= SHA256_F3(S_Block[i]);
			S_Block[i] ^= SHA256_F4(S_Block[i]);
			S_Block[i] |= SHA2_SHFR(S_Block[i], 2);
		}
	}

	for (unsigned i = 0; i < key_size; i++)
		CACHED_KEY[i] ^= S_Block[i];

	return CACHED_KEY;
}

EXPORTED byte* __stdcall CryptResponse(byte* key, unsigned key_size, bool decrypt)
{
	if (key == 0 || key_size > MEMORY_KEY_SIZE)
		return false;

	for (unsigned i = 0; i < MEMORY_SIZE; i++)
		OUT_MEMORY[i] = 0;

	for (unsigned i = 0; i < MEMORY_KEY_SIZE; i++)
		CACHED_KEY[i] = 0;

	for (unsigned i = 0; i < key_size; i++)
		CACHED_KEY[i] = key[i];

	for(unsigned i = 0; i < MEMORY_KEY_SIZE; i++)
		S_Block[i] = (byte)(i % 256);

	for(int i = MEMORY_KEY_SIZE - 1; i >= 0; i--)
	{
		if(i % 2 == 0)
		{
			S_Block[i] ^= SHA256_F1(S_Block[i]);
			S_Block[i] &= SHA256_F2(S_Block[i]);
			S_Block[i] |= SHA256_F3(S_Block[i]);
			S_Block[i] ^= SHA256_F4(S_Block[i]);
			S_Block[i] |= SHA2_ROTR(S_Block[i], 3);
		}
		else
		{
			S_Block[i] ^= SHA256_F4(S_Block[i]);
			S_Block[i] |= SHA256_F3(S_Block[i]);
			S_Block[i] &= SHA256_F2(S_Block[i]);
			S_Block[i] ^= SHA256_F1(S_Block[i]);
			S_Block[i] |= SHA2_SHFR(S_Block[i], 2);
		}
	}

	for(unsigned i = 0; i < key_size; i++)
		CACHED_KEY[i] ^= S_Block[i];

	if(decrypt)
	{
		uint_least32_t crc32 = *((uint_least32_t*)(CACHED_KEY));
		uint_least32_t computed_crc = calc_crc32(&CACHED_KEY[4], key_size - 4);
		if (crc32 != computed_crc)
			return false;
		
	}
	return CACHED_KEY;
}