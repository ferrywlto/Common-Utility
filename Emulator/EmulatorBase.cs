using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;

using com.sky88.common.logging;

namespace com.sky88games.bet.Emulator
{
    public abstract class EmulatorBase
    {
        public const string OK = "OK#";
        //variables from config file
        protected readonly int loop, interval;
        readonly string username, password, logFilename, server_url;
        readonly int tableID;

        //variables NOT from config file
        int accountID;
        string sessionID, prevDisplayStr = String.Empty;
        WebClient web_client;
        ConsoleLogger logger;

        protected int currentGameID, gameNum;
        protected string currentState, webstr_result;
        protected Random rand;

        const string login_url = "common/login.aspx?username={0}&password={1}";
        const string current_table_url = "common/setCurrentTable.aspx?tableID={0}";
        
        public EmulatorBase()
        {
            try
            {
                loop = int.Parse(ConfigurationManager.AppSettings["loop"]);
                interval = int.Parse(ConfigurationManager.AppSettings["interval"]);
                tableID = int.Parse(ConfigurationManager.AppSettings["tableID"]);

                username = ConfigurationManager.AppSettings["username"];
                password = ConfigurationManager.AppSettings["password"];
                logFilename = ConfigurationManager.AppSettings["logFilename"];
                server_url = ConfigurationManager.AppSettings["server_url"];
                
                rand = new Random(DateTime.Now.Millisecond);
                web_client = new WebClient();
                logger = new ConsoleLogger(logFilename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid config file value:" + Environment.NewLine +
                    e.Message + Environment.NewLine + e.StackTrace);
            }
        }
        bool login()
        {
            string url = String.Format(server_url + login_url, username, password);
            webstr_result = web_client.DownloadString(url);
            print(url + " : " + webstr_result);
            bool succeed = checkCallSucceed();
            if (succeed)
            {
                string[] pairs = splitPairs(webstr_result);
                accountID = int.Parse(splitValue(pairs[0])[1]);
                sessionID = splitValue(pairs[1])[1];
            }
            return succeed;
        }
        protected void perform_application_start_logic()
        {
            if (!login()) exit();
            if (!webStrSucceed(String.Format(current_table_url, tableID))) exit();
        }
        public abstract void run();
        protected abstract void resetValues();

        string makeURL(string partialURL)
        {
            if(partialURL.EndsWith("?"))
                return server_url + partialURL + "sessionID=" + sessionID;
            else
                return server_url + partialURL + "&sessionID=" + sessionID;
        }
        protected bool webStrSucceed(string url)
        {
            string result = webstring(url);
            bool succeed = checkCallSucceed(result);
            return succeed;
        }
        protected string webstring(string url)
        {
            webstr_result = web_client.DownloadString(makeURL(url));
            string currDisplayStr = url + " : " + webstr_result;
            if (!currDisplayStr.Equals(prevDisplayStr))
                print(currDisplayStr);
            prevDisplayStr = currDisplayStr;
            return webstr_result;
        }
        protected void exit()
        {
            Console.ReadLine();
            Environment.Exit(0);
        }
        protected void wait(int time)
        {
            Thread.Sleep(time);
        }
        protected void print(string msg)
        {
            logger.printAndLog(msg);
        }
        protected bool checkCallSucceed(string result)
        {
            return result.StartsWith(OK);
        }
        protected bool checkCallSucceed()
        {
            return webstr_result.StartsWith(OK);
        }
        protected string[] splitPairs(string input)
        {
            return input.Substring(input.IndexOf(OK) + 3).Split(';');
        }
        protected string[] splitValue(string input)
        {
            return input.Split(':');
        }

    }
}
