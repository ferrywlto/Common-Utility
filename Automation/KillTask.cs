using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace com.sky88.web.automation
{
    public class KillTask : AutomationTask
    {
        public KillTask(AutomationTaskParameter param) : base(param)
        {
            commandHeader = "kill:";
        }

        protected override void handleDoCommandStep(string process2Kill)
        {
            Process target2Kill = null;
            Process[] procs = Process.GetProcesses();
            foreach (Process p in procs)
            {
                print("process name:" + p.ProcessName);
                if (p.ProcessName.Equals(process2Kill))
                {
                    target2Kill = p;
                    target2Kill.Kill();
                    target2Kill.WaitForExit();
                    break;
                }
            }
            if (target2Kill != null && target2Kill.HasExited) {
                print("Process: " + process2Kill + " has terminated successfully.");
            }
            else
            {
                print("Process: " + process2Kill + " termination failed.");
                throw new Exception("One of the process cannot be killed.");
            }
        }

        protected override void handleNoCommand() { print("No process to kill."); }
    }
}
