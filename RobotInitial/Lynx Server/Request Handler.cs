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

            if (!VM.Initialise()) {
                //Reject connection, virtual machine is busy
                Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection refused, Virtual Machine busy");
                Console.Write("Connection rejected: VM busy \n");
                clientStream.WriteByte(0);
                client.Close();
                return;

            } else {
                //Virtual machine is free and has been allocated to this request
                Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection Accepted");
                Console.Write("Connection accepted \n");
                clientStream.WriteByte(1);
                
                //Deserialise the workspace object from the client stream
                StartBlock program = StartBlock.Deserialise(clientStream);
                Console.Write("Program recieved \n");
                Protocol lynxProtocol = new RobotInitial.LynxProtocol.LynxProtocol();

                //Load the program onto the virtual machine then spawn another thread so we can 
                //continue revieving communication from the client.
                VM.LoadProgram(program, lynxProtocol);
                Thread programThread = new Thread(VM.RunProgram);
                programThread.Start();

                Boolean keepLooping = true;
                while (keepLooping) {                    
                    if (clientStream.DataAvailable) {
                        //DO we have any communications from the client to be processed
                        int message = clientStream.ReadByte();
                                                
                        if (message == 80) {
                            //Pause = 80
                            VM.pause();
                        } else if (message == 82) {
                            //Resume == 82
                            VM.resume();
                        } else if (message == 83) {
                            //Stop == 83
                            VM.TerminateProgram(Shutdown.Software);
                            //programThread.Abort();
                        }
                        
                    } else if (!programThread.IsAlive) {
                        //If the VM is not running, check why
                        if (VM.state == EndState.Completed) {
                            //Send program completed to client
                            clientStream.WriteByte(255);                            
                        } else if (VM.state == EndState.TerminatedByClient) {
                            //Confirm program termination
                            clientStream.WriteByte(10);
                        } 

                        //Reset the VM so another program can run and exit request thread
                        try {
                            VM.Reset();
                        } catch (VirtualMachineOwnershipException) {
                            Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Cannot release VM: Ownership Exception");
                        }

                        keepLooping = false;
                    }

                }

                client.Close();
                return;
            }


        }
    }
}