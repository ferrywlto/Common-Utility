using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(string fileName) : base(fileName)
        {
        }

        public void printAndLogError(string msg, Exception e)
        {
            printError(msg, e);
            logError(msg, e);
        }
        public void printAndLogError(Exception e)
        {
            printError(e);
            logError(e);
        }
        public void printAndLogError(string msg)
        {
            printError(msg);
            logError(msg);
        }

        public void printAndLog(string msg)
        {
            print(msg);
            log(msg);
        }

        public void printError(string msg, Exception e)
        {
            printError(msg + getError(e));
        }
        public void printError(Exception e)
        {
            printError(getError(e));
        }
        public void printError(string msg)
        {
            print("[ERROR]" + msg);
        }
        public void print(string msg)
        {
            Console.WriteLine(getTimestamp() + msg);
        }
    }
}
