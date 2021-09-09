# MP-2020-2021

The 'generating drivers' directory contains all files used to generate and compile the driver files:
- change_sdr.py:   
    path_rd: the template driver file   
    path_wr: the sdr.c file that can be compiled using "$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32"  
- sdr-original.c:
    template sdr.c file
- the 2 .sh files are used to use loops to generate change and compile each individual driver


The 'test header byte values' contains a 'main.cpp' file used to test the generation header byte values with the custom inputs.
beware: Bytes visible in the output are mirrored

The info directory contains:
- pdf of submitted thesis
- txt files with used linux commands
