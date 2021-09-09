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
    [Display("copy sdr.ko to board", Group: "MyAwesomePlugin", Description: "execute given Windows shell command")]
    public class Scp_Driver_To_Board : TestStep
    {
        #region Settings

        [Display("path", Description: "The path to the directory containing the subdirectories")]
        public string path { get; set; }

        // should be inheritend form sweep loop or set manually
        [Display("rate", Description: "inherited value from sweep loop")]
        public int rate { get; set; }

        [Display("reserved", Description: "inherited value from sweep loop")]
        public int reserved { get; set; }

        [Display("length", Description: "inherited value from sweep loop")]
        public int length{ get; set; }

        [Display("tail", Description: "inherited value from sweep loop")]
        public int tail { get; set; }
        //

        //[Display("Command", Description: "The command to give to the Windows command prompt")]
        //public string Command { get; set; }

        #endregion

        public static void test_values(int rate, int reserved, int length, int tail)
        {
            if (rate < -1 || rate > 0x0f)
            {
                throw new ArgumentOutOfRangeException("rate value must be in range [0, 0x0f]");
            }

            if (reserved < -1 || reserved > 1)
            {
                throw new ArgumentOutOfRangeException("reserved value must be 0 or 1");
            }
            if (length < -1 || length > 0xfff)
            {
                throw new ArgumentOutOfRangeException("length value must be in range [0, 0xfff]");
            }
            if (tail < -1 || tail > 0x3f)
            {
                throw new ArgumentOutOfRangeException("tail value must be in range [0, 0x3f]");
            }
        }

        public static void CheckPath(string path, int rate, int reserved, int length, int tail)
        {
            string strCmdText;
            string myDirName = path + "/rate" + rate + " reserved" + reserved + " length" + length + " tail" + tail;
            strCmdText = "/C IF exist " + myDirName;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            // how to use output of cmd command?
            // integrate into pscp command?
        }

        public static void ExecuteCommand(string path, int rate, int reserved, int length, int tail)
        {

            test_values(rate, reserved, length, tail);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C pscp -scp -pw openwifi \"" + path + "/rate" + rate + " reserved" + reserved + " length" + length + " tail" + tail + "/sdr.ko\" root@192.168.10.122:openwifi/";
            //startInfo.Arguments = "/C pscp -scp -pw openwifi "\"D:/test_drivers/rate-1 reserved-1 length-1 tail-1/sdr.ko\"" root@192.168.10.122:openwifi/";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Log.Info(process.StandardOutput.ReadLine());
            }
        }

        public Scp_Driver_To_Board()
        {
            Name = "Send scp Command";
            rate = -1;
            reserved = -1;
            length = -1;
            tail = -1;
            path = "D:/test_drivers";
        }

        public override void Run()
        {
            try{
                ExecuteCommand(path, rate, reserved, length, tail);
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
