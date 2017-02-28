using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

using com.sky88.common.system;

namespace com.sky88.web.automation
{
    public class ExecuteTask : AutomationTask
    {
        public ExecuteTask(AutomationTaskParameter param) : base(param) 
        {
            commandHeader = "execute:";
        }

        protected override void handleDoCommandStep(string file2Execute)
        {
            print("Start execute program: " + file2Execute);
            if (File.Exists(file2Execute))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(file2Execute);
                Process proc = Process.Start(startInfo);
                proc.WaitForExit();
                print("Program: " + file2Execute + " finished.");
            }
            else
            {
                throw new Exception("Target does not exist! Make sure the executable path is correct!");
            }
        }

        protected override void handleNoCommand() { print("No file to execute."); }
    }
}
