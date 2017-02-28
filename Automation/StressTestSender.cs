using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Configuration;

namespace StressTestSender
{
    class StressTestSender
    {
        static void Main(string[] args)
        {
            //========================================================================================================================
            string deployFile = ConfigurationManager.AppSettings["deployList"].ToString();
            string jobFile = ConfigurationManager.AppSettings["jobList"].ToString();


        }

        static void sendCommand()
        {
            TcpClient localClient = new TcpClient();
            localClient.Connect(IPAddress.Parse("localhost"), 8889);

            using (NetworkStream tcpStream = localClient.GetStream())
            {
                byte[] bytes = new ASCIIEncoding().GetBytes("RUN");
                tcpStream.Write(bytes, 0, bytes.Length);
                tcpStream.Flush();
            }
            localClient.Close();
        }

        static void sendFile()
        {
            TcpClient localClient = new TcpClient();
            localClient.Connect(IPAddress.Parse("localhost"), 8888);

            using (NetworkStream tcpStream = localClient.GetStream())
            {
                //uploading file
                using (FileStream stream = new FileStream(@"D:\script.txt", FileMode.Open, FileAccess.Read))
                {
                    // Buffer for reading data
                    byte[] bytes = new byte[1024];
                    while (stream.Read(bytes, 0, bytes.Length) != 0)
                        tcpStream.Write(bytes, 0, bytes.Length);
                }
            }
            localClient.Close();
        }
    }
}
