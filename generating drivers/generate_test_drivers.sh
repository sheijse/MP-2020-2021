#!/bin/bash

export OPENWIFI_DIR=/home/openwifi/openwifi
export XILINX_DIR=/tools/Xilinx
counter = 1

#make a directory where to store the testdirectories containing the changed sdr.sh
mkdir -p /home/openwifi/Desktop/MP/test_drivers
export DIR=/home/openwifi/Desktop/MP/test_drivers
:'
#default values
export rate=-1
export reserved=-1
export length=-1
export tail=-1


#change to original driver and compile
python3 /home/openwifi/Desktop/MP/change_sdr.py
$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32
mkdir -p $DIR/"rate$rate reserved$reserved length$length tail$tail"
cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length$length tail$tail"
cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length$length tail$tail"


#valid rates from lowest to highest
for rate in 13 15 5 7 9 11 1 3
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --rate $rate

  	#store the changed field
  	echo "rate$rate reserved$reserved length$length tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length$length tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length$length tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length$length tail$tail"
  
done

# back to default values for next loop
export rate=-1
export reserved=-1
export length=-1
export tail=-1

#invalid rates from lowest to highest
for rate in 0 2 4 6 8 10 12 14
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --rate $rate

  	#store the changed field
  	echo "rate$rate reserved$reserved length$length tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

  	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length$length tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length$length tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length$length tail$tail"
  
done

# back to default values for next loop
export rate=-1
export reserved=-1
export length=-1
export tail=-1

#varying lengths from lowest to highest
for length in {0..4096..64}
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --length $length

  	#store the changed field
  	echo "rate$rate reserved$reserved length$length tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

  	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length$length tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length$length tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length$length tail$tail"
  
done
'
export rate=-1
export reserved=-1
export length=-1
export tail=-1

#invalid tail values from lowest to highest
for tail in {0..64..2}
do
	let "counter += 1"
  	#change sdr.c 
  	python3 /home/openwifi/Desktop/MP/change_sdr.py --tail $tail

  	#store the changed field
  	echo "rate$rate reserved$reserved length$length tail$tail">> /home/openwifi/Desktop/MP/list_subdirectories.txt

  	# compile driver using openwifi functionalities (see git)
  	$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32

  	#make new dir to contain sdr.ko
  	mkdir -p $DIR/"rate$rate reserved$reserved length$length tail$tail"

  	#copy sdr.ko to a directory:
  	cp $OPENWIFI_DIR/driver/sdr.ko $DIR/"rate$rate reserved$reserved length$length tail$tail"
  	cp $OPENWIFI_DIR/driver/sdr.c $DIR/"rate$rate reserved$reserved length$length tail$tail"
  
done

echo "$counter files generated"

