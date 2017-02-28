using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Collections;
using com.sky88.common.logging;
using com.sky88.common.system;

namespace com.sky88.web.automation
{
    public abstract class AutomationTask
    {
        const string JOB_HEADER = "jobid:";
        const string URL_PATTERN_CMD = "{0}?clientID={1}";
        const string URL_PATTERN_ACK = URL_PATTERN_CMD + "&jobid={2}&status={3}";

        protected readonly WebClient web_client = new WebClient();
        protected readonly AutomationTaskParameter param;
        protected string commandHeader;
        protected bool missionCompleted = false;

        List<string> completedTasks;
        string currentJobID;
        Task task;
        bool keepRunning;

        public bool MissionCompleted { get { return missionCompleted; } }

        public AutomationTask(AutomationTaskParameter param)
        {
            this.param = param;
            this.keepRunning = false;
        }

        public void start()
        {
            completedTasks = new List<string>();
            keepRunning = true;
            task = new Task(() => run());
            try
            {
                task.Start();
            }
            catch (Exception e)
            {
                printError("Caught by start()", e);
                throw e;
            }
        }
        public void wait()
        {
            print("wait called");
            try
            {
                if(task != null) task.Wait();
            }
            catch (Exception e)
            {
                printError("Caught by wait()", e);
                throw e;
            }
        }
        void run()
        {
            print("Task thread starts.");
            try
            {
                while (keepRunning)
                {
                    execute();
                    if (param.IsRepeat)
                    {
                        print("Waiting next cycle...");
                        Thread.Sleep(param.PollInterval);
                    }
                    else
                    {
                        stop();
                        break;
                    }
                }
                print("Terminated gracefully.");
            }
            catch (Exception e)
            {
                param.Observer.notifyError(param.Name + "Terminated due to exception caught by run():", e);
                acknowledgeServerForFail(e.Message);
                wait();
            }
        }

        protected void acknowledgeServerForFail(string errMsg)
        {
            try
            {
                //Acknowledge server the task failed
                web_client.DownloadString(string.Format(URL_PATTERN_ACK, param.AcknowledgeURL, param.ClientID, currentJobID, "ERR:" + errMsg));
            }
            catch (Exception innerEx)
            {
                //Even acknowledgement failed. Means network or server may fail. Have to catch it too.
                param.Observer.notifyError(param.Name + "Exception throw from acknowledgeServerForFail():", innerEx);
            }
        }

        protected void execute()
        {
            print("executing..."); // string cleansing
            string command = web_client.DownloadString(string.Format(URL_PATTERN_CMD, param.CommandURL, param.ClientID)).Trim();
            print("command received=" + command);

            if (command.StartsWith(JOB_HEADER) && command.Contains(commandHeader))
            {
                //check if job already processed
                string jobHeader = command.Substring(0, command.IndexOf(commandHeader)-1).Trim();
                string jobID = jobHeader.Substring(JOB_HEADER.Length);
                
                if (!completedTasks.Contains(jobID))
                {
                    currentJobID = jobID;
                    command = command.Substring(command.IndexOf(commandHeader)).Trim();

                    if (command.StartsWith(commandHeader))
                        handleDoCommand(getCommandTarget(command).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
                    else
                        handleMalformedCommand();
                }
                else handleCompletedCommand();
            }
            else handleNoCommand();
        }

        public void stop() 
        {
            print("Stopping thread.");
            keepRunning = false; 
        }

        protected string getCommandTarget(string command) { return command.Substring(command.IndexOf(commandHeader) + commandHeader.Length); }
        protected void printError(string msg, Exception e) { param.Logger.printAndLogError(param.Name + " " + msg, e); }
        protected void print(string msg = "") { param.Logger.printAndLog(param.Name + " " + msg); }

        protected void handleDoCommand(string[] targetCollection)
        {
            for (int i = 0; i < targetCollection.Length; i++)
            {
                print(string.Format("Job:{0} Step:{1}", currentJobID, i+1));
                handleDoCommandStep(targetCollection[i]);
            }
            try
            {
                if (web_client.DownloadString(string.Format(URL_PATTERN_ACK, param.AcknowledgeURL, param.ClientID, currentJobID, "OK")).Equals("OK"))
                {
                    print(string.Format("Client {0} acknowledged server for job {1} completion.", param.ClientID, currentJobID));
                    missionCompleted = true;
                    completedTasks.Add(currentJobID);
                }
                else throw new Exception("Cannot acknowledge server for task succeed.");
            }
            catch (Exception e) { throw e; }
        }
        abstract protected void handleDoCommandStep(string stepTarget);
        virtual protected void handleNoCommand() { print("No command to process."); }
        virtual protected void handleCompletedCommand() { print("Job already completed."); }
        virtual protected void handleMalformedCommand() { print("Do nothing because of malformed or unknown command."); }
    }
}
