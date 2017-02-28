using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using com.sky88.common.logging;
namespace com.sky88.web.automation
{
    public class SQLTaskParameter :  AutomationTaskParameter
    {
        SqlConnection _conn;

        public SQLTaskParameter(SqlConnection sql_conn = null, string clientID = "", string taskName = "", string commandURL = "", string acknowledgeURL = "",
            Logger logger = null, bool isRepeat = false, int interval = 1000, ITaskObserver observer = null)
            : base(clientID, taskName, commandURL, acknowledgeURL, logger, isRepeat, interval, observer)
        {
            this._conn = sql_conn;
        }

        public SqlConnection Connection
        {
            get { return this._conn; }
            set { this._conn = value; }
        }
    }
}
