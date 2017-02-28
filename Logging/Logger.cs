using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using com.sky88.common.system;

namespace Logging
{
    public class Logger
    {
        string file2Log;
        object mutex = new object();

        public Logger(string fileFullPath)
        {
            log(String.Format("{0} created for {1}", this.GetType().Name, fileFullPath));
            file2Log = fileFullPath;
        }

        public void logError(string msg, Exception e)
        {
            logError(msg + getError(e));
        }
        public void logError(Exception e)
        {
            logError(getError(e));
        }
        public void logError(string msg)
        {
            log("[ERROR] " + msg);
        }
        public void log(string msg)
        {
            try
            {
                lock(mutex) { File.AppendAllText(file2Log, getTimestamp() + msg + Environment.NewLine); }
            }
            catch
            {
                // if buzz that means I/O error, maybe no disk space, or invalid path
                Buzzer.Beep();
            }
        }
        public string getError(Exception e) 
        { 
            if(e.InnerException != null)
                return e.Message + 
                    Environment.NewLine + e.StackTrace + 
                    Environment.NewLine + "[INNER EXCEPTION]" +
                    Environment.NewLine + getError(e.InnerException); 
            else
                return e.Message + Environment.NewLine + e.StackTrace; 
        }
        public string getTimestamp() { return DateTime.Now.ToString("[yyyyMMdd-HHmmss-ffff] "); }
    }
}
