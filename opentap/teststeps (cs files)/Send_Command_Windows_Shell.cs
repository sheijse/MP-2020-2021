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
    [Display("Send Command to Command Prompt (Windows)", Group: "MyAwesomePlugin", Description: "execute given Windows shell command")]
    public class Send_Command_Windows_Shell : TestStep
    {
        #region Settings

        [Display("Command", Description: "The command to give to the Windows command prompt")]
        public string Command { get; set; }

        #endregion

        public static void ExecuteCommand(string command)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                Log.Info(process.StandardOutput.ReadLine());
            }
        }

        public Send_Command_Windows_Shell()
        {
            Name = "Send Command";
            Command = "";
        }

        public override void Run()
        {
            string tmpCommand;
            tmpCommand = Command;
            try{
                ExecuteCommand(Command);
            }
            catch (Exception e)
            {
                Log.Warning(e.Message);
            }
            
        }
    }
}
