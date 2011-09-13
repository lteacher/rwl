using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using RobotInitial.Model;
using System.Threading;
using System.Net;

namespace RobotInitial.Lynx_Server {
    class Request_Handler {

        TcpClient client;

        public Request_Handler(TcpClient client) {
            this.client = client;
        }       

        public void processRequest() {
            //Grab a handle to the virtual machine
            Virtual_Machine VM = Virtual_Machine.Instance;
            NetworkStream clientStream = client.GetStream();

            if (!VM.Initialise(client, Thread.CurrentThread.ManagedThreadId)) {
                //Reject connection, virtual machine is busy
                Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection refused, Virtual Machine busy");
                clientStream.WriteByte(0);
                client.Close();
                return;

            } else {
                //Virtual machine is free and has been allocated to this request
                Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection Accepted");
                clientStream.WriteByte(1);
                
                //Deserialise the workspace object from the client stream
                Workspace program = Workspace.Deserialise(clientStream);

                //Load the program onto the virtual machine then spawn another thread so we can 
                //continue revieving communication from the client.
                VM.LoadProgram(program);
                Thread programThread = new Thread(VM.RunProgram);
                programThread.Start();

                Boolean keepLooping = true;
                while (keepLooping) {                    
                    if (clientStream.DataAvailable) {
                        //DO we have any communications from the client to be processed

                    } else if (!programThread.IsAlive) {
                        //If the VM is not running, check why
                        if (VM.state == EndState.Completed) {
                            //Send program completed to client                            
                        } else if (VM.state == EndState.TerminatedByClient) {
                            //Confirm program termination
                        } else if (VM.state == EndState.TerminatedByHardware) {
                            //Send program terminated via hardware error to client
                        }

                        //Reset the VM so another program can run and exit request thread
                        VM.Reset();
                        keepLooping = false;
                    }

                }

                client.Close();
                return;
            }


        }
    }
}













//Some manual message recieving code incase its needed.
//byte[] messageLengthByte = new byte[4];
//clientStream.Read(messageLengthByte, 0, 4);

////Convert 4 byte binary to integer
//int messageLengthInt = BitConverter.ToInt32(messageLengthByte, 0);
//byte[] message = new byte[messageLengthInt];
//int recievedBytes = 0;

////Loop while we have not recieved the entire message
//while (recievedBytes < messageLengthInt) {
//    //Read message length bytes minus what has already been read, into the message byte array at next empty position
//    recievedBytes += clientStream.Read(message, recievedBytes, (messageLengthInt - recievedBytes));
//}
