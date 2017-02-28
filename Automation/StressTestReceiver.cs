using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StressTestReceiver
{
    internal class StressTestReceiver
    {
        private static ArrayList processNames = new ArrayList();

        private static void Main(string[] args)
        {
        }

        private static void receiveCommand()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("localhost"), 8889);
            listener.Start();
            string command = string.Empty;
            while (true)
            {
                command = parseCommand(listener);
                if (command.Equals("RUN"))
                {
                    // read script file
                    TextReader tr = new StreamReader("");
                    string cmd = tr.ReadLine();
                    if (cmd.StartsWith(""))
                    {
                    }
                    //run script
                    //Process proc = Process.Start("");
                    //proc.ProcessName;

                    else if (command.Equals("KILLALL"))
                    {
                        foreach (Process proc in Process.GetProcesses())
                        {
                            //if(proc.ProcessName =
                        }
                    }
                }
                else if (command.Equals("SHUTDOWN"))
                {
                    break;
                }
            }

            listener.Stop();
        }

        private static string parseCommand(TcpListener listener)
        {
            string receivedCommand = string.Empty;
            using (TcpClient localClient = listener.AcceptTcpClient())
            {
                using (NetworkStream tcpStream = localClient.GetStream())
                {
                    Decoder decoder = Encoding.UTF8.GetDecoder();
                    byte[] buffer = new byte[1024];
                    int bufferBytesRead = tcpStream.Read(buffer, 0, buffer.Length);

                    char[] outputChars = new char[bufferBytesRead];
                    decoder.GetChars(buffer, 0, bufferBytesRead, outputChars, 0);
                    receivedCommand = new string(outputChars);
                }
            }
            return receivedCommand;
        }

        private static void receiveFile()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("localhost"), 8888);
            listener.Start();

            TcpClient localClient;
            localClient = listener.AcceptTcpClient();
            while (true)
            {
                NetworkStream tcpStream = localClient.GetStream();
                //saving file
                using (FileStream stream = new FileStream(@"D:\script.txt", FileMode.Create, FileAccess.ReadWrite))
                {
                    // Buffer for reading data
                    byte[] bytes = new byte[1024];

                    while (tcpStream.Read(bytes, 0, bytes.Length) != 0)
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                localClient.Close();
                if (listener.Pending()) { localClient = listener.AcceptTcpClient(); }
            }
            listener.Stop();
        }
    }
}