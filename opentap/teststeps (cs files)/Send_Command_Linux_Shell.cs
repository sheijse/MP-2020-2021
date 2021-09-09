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
    [Display("Send Command to linux Terminal", Group: "MyAwesomePlugin", Description: "execute given linux shell command")]
    public class Send_Command_Linux_Shell : TestStep
    {
        #region Settings

        [Display("Command", Description: "The command to give to the Linux shell")]
        public string Command { get; set; }

        #endregion

        public static void ExecuteCommand(string command)
        {
            Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "/bin/bash";
            process.StartInfo.Arguments = "-c \" " + command + " \"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Console.WriteLine(process.StandardOutput.ReadLine());
            }
        }

        public Send_Command_Linux_Shell()
        {
            Name = "Send Command";
            Command = "";
        }

        public override void Run()
        {
            string tmpCommand;
            tmpCommand = Command;
            ExecuteCommand(Command);
        }
    }
}
