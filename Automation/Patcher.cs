using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using com.sky88.web.automation;
using com.sky88.common.logging;
using com.sky88.common.system;

namespace Patcher
{
    public class Patcher : ITaskObserver
    {
        string urlKill = string.Empty;
        string urlKillAck = string.Empty;
        string urlDownload = string.Empty;
        string urlDownloadAck = string.Empty;
        string urlExecute = string.Empty;
        string urlExecuteAck = string.Empty;
        string clientID = string.Empty;
        string logPath = string.Empty;
        string downloadPath = string.Empty;

        public Patcher()
        {
            try
            {
                urlKill = ConfigurationManager.AppSettings["urlKill"].ToString();
                urlKillAck = ConfigurationManager.AppSettings["urlKillAck"].ToString();
                urlDownload = ConfigurationManager.AppSettings["urlDownload"].ToString();
                urlDownloadAck = ConfigurationManager.AppSettings["urlDownloadAck"].ToString();
                urlExecute = ConfigurationManager.AppSettings["urlExecute"].ToString();
                urlExecuteAck = ConfigurationManager.AppSettings["urlExecuteAck"].ToString();
                clientID = ConfigurationManager.AppSettings["clientID"].ToString();
                logPath = ConfigurationManager.AppSettings["logPath"].ToString();
                downloadPath = ConfigurationManager.AppSettings["downloadPath"].ToString();
            }
            catch
            {
                MessageBox.Show("Invalid config file value, please check.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        public void run()
        {
            Console.WriteLine("patch start");
            string name = "[PATCH]";
            Logger logger = new Logger(logPath + name + DateTime.Now.ToString("yyyyMMdd") + ".log");

            AutomationTaskParameter atpKill = new AutomationTaskParameter();
            atpKill.ClientID = clientID;
            atpKill.AcknowledgeURL = urlKillAck;
            atpKill.Name = name;
            atpKill.Logger = logger;
            atpKill.IsRepeat = false;
            atpKill.CommandURL = urlKill;
            atpKill.Observer = this;

            KillTask taskKill = new KillTask(atpKill);
            taskKill.start();

            if (taskKill.MissionCompleted)
            {
                Console.WriteLine("kill succeed. getting download command.");
                AutomationTaskParameter atpDownload = new AutomationTaskParameter();
                atpDownload.ClientID = clientID;
                atpDownload.AcknowledgeURL = urlDownloadAck;
                atpDownload.Name = name;
                atpDownload.Logger = logger;
                atpDownload.IsRepeat = false;
                atpDownload.CommandURL = urlDownload;
                atpDownload.Observer = this;

                DownloadTaskParameter dtp = new DownloadTaskParameter(downloadPath);
                DownloadTask taskDownload = new DownloadTask(atpDownload, dtp);
                taskDownload.start();

                if (taskDownload.MissionCompleted)
                {
                    Console.WriteLine("download succeed. getting execution command.");
                    AutomationTaskParameter atpExec = new AutomationTaskParameter();
                    atpExec.ClientID = clientID;
                    atpExec.AcknowledgeURL = urlExecuteAck;
                    atpExec.Name = name;
                    atpExec.Logger = logger;
                    atpExec.IsRepeat = false;
                    atpExec.CommandURL = urlExecute;
                    atpExec.Observer = this;

                    ExecuteTask taskExec = new ExecuteTask(atpExec);
                    taskExec.start();

                    if (taskExec.MissionCompleted)
                        Console.WriteLine("patch complete.");
                }
            }
            logger.print("press <Enter> to exit.");
            Console.ReadLine();
        }

        public void notifyProgress(string message) { }
        public void notifyError(string message, Exception e)
        {
            Console.WriteLine("Program terminated due to exeception occured:" + message, e);
            Buzzer.Beep();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            Patcher p = new Patcher();
            p.run();
        }
    }
}
