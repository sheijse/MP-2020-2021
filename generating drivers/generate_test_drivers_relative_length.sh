#!/bin/bash

export OPENWIFI_DIR=/home/openwifi/openwifi
export XILINX_DIR=/tools/Xilinx
counter = 1

#make a directory where to store the testdirectories containing the changed sdr.sh
mkdir -p /home/openwifi/Desktop/MP/test_drivers
export DIR=/home/openwifi/Desktop/MP/test_drivers

#default values
export rate=-1
export reserved=-1
export length=-1
export tail=-1

#relative lengths
for val in -100 -50 -20 -10 10 20 50 100
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --length_rel $val --tail $tail

  	#store the changed field
  	echo "rate$rate reserved$reserved length_rel$val tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"
  
done

#default values
export rate=-1
export reserved=-1
export length=-1
export tail=63

#relative lengths with changed tail
for val in -100 -50 -20 -10 10 20 50 100
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --length_rel $val --tail $tail

  	#store the changed field
  	echo "rate$rate reserved$reserved length_rel$val tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length_rel$val tail$tail"
  
done

echo "$counter files generated"
