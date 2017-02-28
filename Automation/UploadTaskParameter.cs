using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.sky88.common.logging;

namespace com.sky88.web.automation
{
    public class UploadTaskParameter : AutomationTaskParameter
    {
        string _path;

        public UploadTaskParameter(string url,string clientID = "", string taskName = "", string commandURL = "", string acknowledgeURL = "",
            Logger logger = null, bool isRepeat = false, int interval = 1000, ITaskObserver observer = null)
            : base(clientID, taskName, commandURL, acknowledgeURL, logger, isRepeat, interval, observer)
        {
            _path = url;
        }

        public string PathUploadTo
        {
            get { return _path; }
            set { _path = value; }
        }
    }
}
