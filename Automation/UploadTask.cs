using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.sky88.web.automation
{
    public class UploadTask : AutomationTask
    {
        UploadTaskParameter utp;
        public UploadTask(AutomationTaskParameter param, UploadTaskParameter upParam) : base(param)
        {
            utp = upParam;
            commandHeader = "upload:";
        }

        protected override void handleDoCommandStep(string file2Upload)
        {
            byte[] uploadResult = web_client.UploadFile(utp.PathUploadTo, file2Upload);
            string uploadResultStr = System.Text.Encoding.ASCII.GetString(uploadResult);
                
            if (uploadResultStr.Equals("OK"))
                print(string.Format("File {0} uploaded to {1} successfully.", file2Upload, utp.PathUploadTo));
            else 
                throw new Exception("Upload file failed. Return code not equals to 'OK'");
        }

        protected override void handleNoCommand() { print("No file to upload."); }
    }
}
