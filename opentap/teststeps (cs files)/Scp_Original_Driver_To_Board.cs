// Author: Steven H
// Copyright:   Copyright 2021 Keysight Technologies
//              You have a royalty-free right to use, modify, reproduce and distribute
//              the sample application files (and/or any modified version) in any way
//              you find useful, provided that you agree that Keysight Technologies has no
//              warranty, obligations or liability for any sample application files.
using OpenTap;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyAwesomePlugin
{
    [Display("copy original driver sdr.ko to board", Group: "MyAwesomePlugin", Description: "Copy the original openwifi driver to board")]
    public class Scp_Original_Driver_To_Board : TestStep
    {
        #region Settings

        [Display("path", Description: "The path to the directory containing the subdirectories")]
        public string path { get; set; }

        //[Display("Command", Description: "The command to give to the Windows command prompt")]
        //public string Command { get; set; }

        #endregion


        public static void ExecuteCommand(string path)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C pscp -scp -pw openwifi \"" + path + "/original/sdr.ko\" root@192.168.10.122:openwifi/";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Log.Info(process.StandardOutput.ReadLine());
            }
        }

        public Scp_Original_Driver_To_Board()
        {
            Name = "Copy original openwifi driver to board";
            path = "D:/test_drivers";
        }

        public override void Run()
        {
            try{
                ExecuteCommand(path);
                UpgradeVerdict(Verdict.Pass);
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
                UpgradeVerdict(Verdict.Error);
            }
            
        }
    }
}
