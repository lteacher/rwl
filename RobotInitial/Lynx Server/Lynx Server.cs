using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace RobotInitial.Lynx_Server
{
    class Lynx_Server
    {
        private TcpListener tcpListener;
        private Thread requestThread;
        private Request_Handler currentRequest = new Request_Handler();
        private TcpClient client;

        private static TextWriter logFile = File.AppendText("log.txt");

        public Lynx_Server(){                      
        }

        public void start() {
            //Listen for connects on Any of the devices network connections
            tcpListener = new TcpListener(IPAddress.Any, 7331);
            tcpListener.Start();

            while (true) {
                //Check for TCP/IP connection requests
                client = tcpListener.AcceptTcpClient();

                //Send request off to a new thread to be handled
                currentRequest.setClient(client);
                currentRequest.processRequest();
                //requestThread = new Thread(currentRequest.processRequest);
                //requestThread.Start();
            }  
        }

        public static void Log(string message){
            logFile.Write(message);
        }

        public static string getIPAddress(TcpClient client) {
            return "" + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
        }
    }
}
