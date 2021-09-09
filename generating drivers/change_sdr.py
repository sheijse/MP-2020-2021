# -*- coding: utf-8 -*-
"""
Created on Sat May  8 14:56:52 2021

@author: Steven

This file remakes the sdr.c file given in path_wr by reading the template sdr.c and changing the field values

//KEYWORD
rate = SIG_RATE;	//0x00 to 0x0f or 0 to 15
reserved = 0x00;	//0x10 or 0x00
length = l_len;		//0 to 0xFFF
tail = 0x00;		//0 to 63 or 0x0 to 0x3f
rate_changed = 0;
//END_KEYWORD

"""
#replacing in the same file or making new file:
#https://pythonexamples.org/python-replace-string-in-file/

import sys
import argparse

# function that can be used to mirror a byte
def mirror_byte(b):
    return int('{:08b}'.format(b)[::-1], 2)

# should allow for bin, hex and dec inputs
def auto_int(x):
        return int(x, 0)
# use type = lambda x: int(x,0) if u don't want to use this function

parser = argparse.ArgumentParser(description='input parameters Openwifi change sdr.c')

# allows decimal input, hex input (0xff) and binary input (0b1010)

parser.add_argument('--path_rd'  , type=str     , default = '/home/openwifi/Desktop/MP/sdr-original.c', help='pathname to original sdr.c that will be used as template but not changed')
parser.add_argument('--path_wr'  , type=str     , default = '/home/openwifi/openwifi/driver/sdr.c', help='pathname to sdr.c that will be changed and compiled')
parser.add_argument('--rate'    , type=auto_int, default = -1, choices=range(0x00, 0x0f +1)   , help='rate field of PLCP header, value between 0x00 and 0x0f')
parser.add_argument('--reserved', type=auto_int, default = -1, choices=[0x00, 0x10]           , help='reserved bit of PLCP header, value 0x00 or 0x10')
parser.add_argument('--length'  , type=auto_int, default = -1, choices=range(0x000, 0xfff +1) , help='length field of PLCP header, value between 0x000 and 0xfff')  
parser.add_argument('--length_rel'  , type=auto_int, default = 0, choices=range(-0xfff, 0xfff +1) , help='value added to default l_len')  
parser.add_argument('--tail'    , type=auto_int, default = -1, choices=range(0x00, 0x3f +1)   , help='tail field of PLCP header, value between 0x00 and 0x3f') 

args = parser.parse_args()

if( args.length != -1 and args.length_rel != 0):
    raise Exception('cannot change both length variables')

#fin = open('/home/openwifi/Desktop/MP/sdr-original.c','rt')
fin = open(args.path_rd,'rt')

#read file contents to string (not the most eff. but the easiest)
data = fin.read()

#replace all occurrences of the required string
if(args.rate != -1):
    data = data.replace('rate = SIG_RATE'   , f'rate = {args.rate}')
    data = data.replace('rate_changed = 0'   , f'rate_changed = 1')
if(args.reserved != -1):
    data = data.replace('reserved = 0x00'   , 'reserved = 0x10') #set reserved field to 0001000 then it does not have to be mirrored
if(args.length != -1):
    data = data.replace('length = l_len'    , f'length = {args.length}')
if(args.tail != -1):
    data = data.replace('tail = 0x00'       , f'tail = {args.tail}')

if(args.length_rel != 0):
    data = data.replace('length = l_len'    , f'length = l_len + {args.length_rel}')

#close the input file
fin.close()

#open the input file in write mode
#fin = open('/home/openwifi/openwifi/driver/sdr.c','wt')
fin = open(args.path_wr,'wt')
#overrite the input file with the resulting data
fin.write(data)
#close the file
fin.close()
