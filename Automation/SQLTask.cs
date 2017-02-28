using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
namespace com.sky88.web.automation
{
    public class SQLTask : AutomationTask
    {
        SQLTaskParameter stp;
        public SQLTask(AutomationTaskParameter param, SQLTaskParameter sqlParam) : base(param) 
        {
            stp = sqlParam;
            commandHeader = "sql:";
        }

        protected override void handleDoCommandStep(string sql2Excute)
        {
            print("SQL to run: " + sql2Excute);
            int resultCount;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = stp.Connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql2Excute;
            stp.Connection.Open();

            try { resultCount = cmd.ExecuteNonQuery(); }
            catch (Exception e) { print("Error occur when execute SQL statement: " + sql2Excute); throw e; }
            finally { if (stp.Connection.State == ConnectionState.Open) stp.Connection.Close(); }

            if (resultCount >= 0)
                print("SQL execution succeed.");
            else
                throw new Exception("SQL result count < 0");
        }

        protected override void handleNoCommand() { print("No SQL to run."); }
    }
}
