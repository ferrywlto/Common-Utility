using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.sky88.common.logging;

namespace com.sky88.web.automation
{
    public class ExecuteTaskParameter : AutomationTaskParameter 
    {
        public ExecuteTaskParameter(string clientID = "", string taskName = "", string commandURL = "", string acknowledgeURL = "",
            Logger logger = null, bool isRepeat = false, int interval = 1000, ITaskObserver observer = null)
            : base(clientID, taskName, commandURL, acknowledgeURL, logger, isRepeat, interval, observer)
        {
        }
    }
}
