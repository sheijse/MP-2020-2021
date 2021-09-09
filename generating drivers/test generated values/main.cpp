#include <stdio.h>
#include <iostream>
//#include <cstring>
#include <string.h>
#include <bitset>

typedef unsigned int u32;
typedef unsigned short int u16;
typedef char u8;
typedef unsigned long int u64;

static const u8  wifi_mcs_table_11b_force_up[16] = { 11, 11, 11,  11, 11, 15,  10,  14,   9,  13,   8,  12,   0,   0,   0,  0 };
static const u16 wifi_n_dbps_ht_table[16] = { 26, 26, 26,  26, 26, 52,  78, 104, 156, 208, 234, 260,   0,   0,   0,  0 };

u8 gen_ht_sig_crc(u64 m)
{
	u8 i, temp, c[8] = { 1, 1, 1, 1, 1, 1, 1, 1 }, ht_sig_crc;

	for (i = 0; i < 34; i++)
	{
		temp = c[7] ^ ((m >> i) & 0x01);

		c[7] = c[6];
		c[6] = c[5];
		c[5] = c[4];
		c[4] = c[3];
		c[3] = c[2];
		c[2] = c[1] ^ temp;
		c[1] = c[0] ^ temp;
		c[0] = temp;
	}
	ht_sig_crc = ((~c[7] & 0x01) << 0) | ((~c[6] & 0x01) << 1) | ((~c[5] & 0x01) << 2) | ((~c[4] & 0x01) << 3) | ((~c[3] & 0x01) << 4) | ((~c[2] & 0x01) << 5) | ((~c[1] & 0x01) << 6) | ((~c[0] & 0x01) << 7);

	return ht_sig_crc;
}

u32 gen_parity(u32 v) {
	v ^= v >> 1;
	v ^= v >> 2;
	v = (v & 0x11111111U) * 0x11111111U;
	return (v >> 28) & 1;
}

u32 calc_phy_header(u8 rate_hw_value, bool use_ht_rate, bool use_short_gi, u32 len, u8* bytes) {
	//u32 signal_word = 0 ;
	u8  SIG_RATE = 0, HT_SIG_RATE;
	u8	len_2to0, len_10to3, len_msb, b0, b1, b2, header_parity;
	u32 l_len, ht_len, ht_sig1, ht_sig2;

	u8 	rate, reserved, tail;
	u32 length;

	// printk("rate_hw_value=%u\tuse_ht_rate=%u\tuse_short_gi=%u\tlen=%u\n", rate_hw_value, use_ht_rate, use_short_gi, len);
	// HT-mixed mode ht signal

	if (use_ht_rate)
	{
		SIG_RATE = wifi_mcs_table_11b_force_up[4];
		HT_SIG_RATE = rate_hw_value;
		l_len = 24 * len / wifi_n_dbps_ht_table[rate_hw_value];
		ht_len = len;
	}
	else
	{
		// rate_hw_value = (rate_hw_value<=4?0:(rate_hw_value-4));
		// SIG_RATE = wifi_mcs_table_phy_tx[rate_hw_value];
		SIG_RATE = wifi_mcs_table_11b_force_up[rate_hw_value];
		l_len = len;
	}

	l_len = 0xfff;
	std::cout << "SIG_RATE: " << std::bitset<8>(SIG_RATE) << std::endl;
	std::cout << "l_len: " << std::bitset<12>(l_len) << std::endl;

	std::cout << "original code" << std::endl;
	// original
	len_2to0 = l_len & 0x07;
	len_10to3 = (l_len >> 3) & 0xFF;
	len_msb = (l_len >> 11) & 0x01;
	std::cout << "msb: " << std::bitset<8>(len_msb) << std::endl;

	b0 = SIG_RATE | (len_2to0 << 5);
	b1 = len_10to3;
	header_parity = gen_parity((len_msb << 16) | (b1 << 8) | b0);
	b2 = (len_msb | (header_parity << 1));

	std::cout << "b0: " << std::bitset<8>(b0) << std::endl;
	std::cout << "b1: " << std::bitset<8>(b1) << std::endl;
	std::cout << "b2: " << std::bitset<8>(b2) << std::endl;

	//printk("normal values:");
	//printk("rate=%u\treserved=%u\tlength=%u\ttail=%u\n", SIG_RATE, 0, l_len, 0);
	//printk("bytes 1-3 in memory:);
	//printk("byte 0 = %u\t byte 1 = %u\t byte 2 = %u", b0, b1, b2);

	std::cout << std::endl;
	std::cout <<"changed code" << std::endl;
	//KEYWORD
	rate = SIG_RATE;			//0x00 to 0x0f or 0 to 15
	reserved = 0x00;	//0x10 or 0x00
	length = l_len + -10;		//0 to 4095
	tail = 0x01;		//0 to 63 or 0x0 to 0x3f
	//END_KEYWORD

	std::cout << "rate: "		<< std::bitset<8>(rate) << std::endl;
	std::cout << "reserved: "	<< std::bitset<8>(reserved) << std::endl;
	std::cout << "length: "		<< std::bitset<12>(length) << std::endl;
	std::cout << "tail: "		<< std::bitset<8>(tail) << std::endl;

	// mirror tail bits to get expected result in header (tail and rate need to be mirror to get expected header)
	
	tail = (tail & 0xF0) >> 4 | (tail & 0x0F) << 4;
	tail = (tail & 0xCC) >> 2 | (tail & 0x33) << 2;
	tail = (tail & 0xAA) >> 1 | (tail & 0x55) << 1;
	
	if (rate != SIG_RATE) {
		rate = (rate & 0xF0) >> 4 | (rate & 0x0F) << 4;
		rate = (rate & 0xCC) >> 2 | (rate & 0x33) << 2;
		rate = (rate & 0xAA) >> 1 | (rate & 0x55) << 1;
		rate = rate >> 4;
	}
	
	//changed
	len_2to0 = length & 0x07;
	len_10to3 = (length >> 3) & 0xFF;
	len_msb = (length >> 11) & 0x01;
	std::cout << "msb: " << std::bitset<8>(len_msb) << std::endl;

	b0 = (rate) | reserved |(len_2to0 << 5);
	b1 = len_10to3;
	header_parity = gen_parity((len_msb << 16) | (b1 << 8) | b0);
	b2 = (len_msb | (tail) | (header_parity << 1)); //(len_msb | (tail) | (header_parity << 1));

	std::cout << "b0: " << std::bitset<8>(b0) << std::endl;
	std::cout << "b1: " << std::bitset<8>(b1) << std::endl;
	std::cout << "b2: " << std::bitset<8>(b2) << std::endl;

	//printk("altered values:");
	//printk("rate=%u\treserved=%u\tlength=%u\ttail=%u\n", rate, reserved, length, tail);
	//printk("bytes 1-3 in memory:);
	//printk("byte 0 = %u\t byte 1 = %u\t byte 2 = %u", b0, b1, b2);

	memset(bytes, 0, 16);
	bytes[0] = b0;
	bytes[1] = b1;
	bytes[2] = b2;

	//HT-mixed mode signal
	if (use_ht_rate)
	{
		ht_sig1 = (HT_SIG_RATE & 0x7F) | ((ht_len << 8) & 0xFFFF00);
		ht_sig2 = 0x04 | (use_short_gi << 7);
		ht_sig2 = ht_sig2 | (gen_ht_sig_crc(ht_sig1 | ht_sig2 << 24) << 10);

		bytes[3] = 1;
		bytes[8] = (ht_sig1 & 0xFF);
		bytes[9] = (ht_sig1 >> 8) & 0xFF;
		bytes[10] = (ht_sig1 >> 16) & 0xFF;
		bytes[11] = (ht_sig2 & 0xFF);
		bytes[12] = (ht_sig2 >> 8) & 0xFF;
		bytes[13] = (ht_sig2 >> 16) & 0xFF;

		return(HT_SIG_RATE);
	}
	else
	{
		//signal_word = b0+(b1<<8)+(b2<<16) ;
		//return signal_word;
		return(SIG_RATE);
	}
}

int main(int argc, char* argv[]) {
	u8 bytes = 0;
	calc_phy_header(0, false, false, 0, &bytes);
	return 0;
}