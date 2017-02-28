using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sky88.web.automation
{
    public class DownloadTask : AutomationTask
    {
        DownloadTaskParameter dtp;
        public DownloadTask(AutomationTaskParameter param, DownloadTaskParameter dlParam) : base(param)
        {
            dtp = dlParam;
            commandHeader = "download:";
        }

        protected override void handleDoCommandStep(string file2Download)
        {
            print("Trying to download..." + file2Download);
            try
            {
                string fileSaveAs = dtp.PathDownloadTo + file2Download.Substring(file2Download.LastIndexOf("/") + 1);
                web_client.DownloadFile(file2Download, fileSaveAs);
                print(string.Format("File {0} downloaded as {1} successfully.", file2Download, fileSaveAs));
            }
            catch (Exception e)
            {
                print("File download failed.");
                throw e;
            }
        }

        protected override void handleNoCommand() { print("No file to download."); }
    }
}
