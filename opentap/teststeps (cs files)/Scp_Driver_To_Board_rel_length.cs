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
    [Display("copy relative length sdr.ko to board", Group: "MyAwesomePlugin", Description: "execute given Windows shell command")]
    public class Scp_Driver_To_Board_rel_length : TestStep
    {
        #region Settings

        // should be inheritend form sweep loop or set manually
        [Display("rate", Description: "inherited value from sweep loop")]
        public int rate { get;}

        [Display("reserved", Description: "inherited value from sweep loop")]
        public int reserved { get; }

        [Display("path", Description: "The path to the directory containing the subdirectories")]
        public string path { get; set; }

        [Display("rel_length", Description: "value added to l_len in driver")]
        public int rel_length { get; set; }

        [Display("tail", Description: "inherited value from sweep loop")]
        public int tail { get; set; }

        #endregion

        public static void test_values( int rel_length, int tail)
        {
            
            if (rel_length < -0xfff || rel_length > 0xfff)
            {
                throw new ArgumentOutOfRangeException("length value must be in range [0, 0xfff]");
            }
            if (tail < -1 || tail > 0x3f)
            {
                throw new ArgumentOutOfRangeException("tail value must be in range [0, 0x3f]");
            }
        }

        public static void CheckPath(string path, int rel_length, int tail)
        {
            string strCmdText;
            string myDirName = path + "/rate" + -1 + " reserved" + -1 + " length_rel" + rel_length + " tail" + tail;
            strCmdText = "/C IF exist " + myDirName;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            // how to use output of cmd command?
            // integrate into pscp command?
        }

        public static void ExecuteCommand(string path, int rel_length, int tail)
        {

            test_values(rel_length, tail);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C pscp -scp -pw openwifi \"" + path + "/rate"+ -1 +" reserved" + -1 + " length_rel" + rel_length + " tail" + tail + "/sdr.ko\" root@192.168.10.122:openwifi/";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Log.Info(process.StandardOutput.ReadLine());
            }
        }

        public Scp_Driver_To_Board_rel_length()
        {
            Name = "Send scp Command";
            rel_length = 0;
            tail = -1;
            path = "D:/test_drivers";
        }

        public override void Run()
        {
            try{
                ExecuteCommand(path, rel_length, tail);
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
