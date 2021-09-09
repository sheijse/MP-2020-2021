# MP-2020-2021

The 'generating drivers' directory contains all files used to generate and compile the driver files:
- change_sdr.py:   
    path_rd: the template driver file   
    path_wr: the sdr.c file that can be compiled using "$OPENWIFI_DIR/driver/make_all.sh $OPENWIFI_DIR $XILINX_DIR 32"  
- sdr-original.c:
    template sdr.c file
- the 2 .sh files are used to use loops to generate change and compile each individual driver

The 'opentap' directory contains 2 subdirectories:
- 1 for the testplans 
- 1 for the .cs files used to make custom teststeps

To use the teststeps you have to .cs files in a project as mentioned in the guide and run it using Visual Studio.   
When it fails a dll file will be created in the \bin\Debug subdirectory. This dll needs to be placed in the root directory of OpenTAP.  

The 'test header byte values' contains a 'main.cpp' file used to test the generation header byte values with the custom inputs.
beware: Bytes visible in the output are mirrored.  

The info directory contains:
- pdf of submitted thesis and defense presentation
- txt files with used linux commands

usefull urls:
most updated version of the thesis    
https://www.overleaf.com/read/jdxcrsqmmzzx  

usefull links regarding opentap  
https://doc.opentap.io/Developer%20Guide/Introduction/    
https://gitlab.com/OpenTAP/Plugins/university-of-malaga/uma-android  
https://gitlab.com/OpenTAP/Plugins/keysight/sshsteps/-/blob/master/OpenTap.Plugins.Ssh/SshCommandStep.cs  

