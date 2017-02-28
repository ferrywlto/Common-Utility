using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.sky88.common.logging;

namespace com.sky88.web.automation
{
    public class DownloadTaskParameter : AutomationTaskParameter
    {
        string _path;

        public DownloadTaskParameter(string path, string clientID = "", string taskName = "", string commandURL = "", string acknowledgeURL = "",
            Logger logger = null, bool isRepeat = false, int interval = 1000, ITaskObserver observer = null)
            : base(clientID, taskName, commandURL, acknowledgeURL, logger, isRepeat, interval, observer)
        {
            this._path = path;
        }

        public string PathDownloadTo
        {
            get { return this._path; }
            set { this._path = value; }
        }
    }
}
