using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.sky88.common.logging;

namespace com.sky88.web.automation
{
    public class AutomationTaskParameter
    {
        int _interval;
        string _client_id;
        string _url_cmd;
        string _url_ack;
        string _name;
        bool _repeat;
        Logger _logger;
        ITaskObserver _observer;

        public AutomationTaskParameter(string clientID = "", string taskName = "", string commandURL = "", string acknowledgeURL = "", 
            Logger logger = null, bool isRepeat = false, int interval = 1000, ITaskObserver observer = null)
        {
            _client_id = clientID;
            _name = taskName;
            _interval = interval;
            _url_cmd = commandURL;
            _url_ack = acknowledgeURL;
            _logger = logger;
            _repeat = isRepeat;
            _observer = observer;
        }
        public ITaskObserver Observer
        {
            get { return _observer; }
            set { _observer = value; }
        }

        public string ClientID
        {
            get { return _client_id; }
            set { _client_id = value; }
        }
        public bool IsRepeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }
        public int PollInterval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        public string AcknowledgeURL
        {
            get { return _url_ack; }
            set { _url_ack = value; }
        }
        public string CommandURL
        {
            get { return _url_cmd; }
            set { _url_cmd = value; }
        }

        public Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
