using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using RobotInitial.Model;
using System.Threading;
using System.Net;

namespace RobotInitial.Lynx_Server {
    class Request_Handler {
        
        //REQUESTS
        public const byte SENDPROG = 0;
        public const byte STOP = 83;
        public const byte PAUSE = 80;
        public const byte RESUME = 82;
        public const byte STATUS = 2;
        public const byte DISCON = 77;

        //GOOD RESPONSES
        public const byte ACK = 1;

        //ERROR RESPONSES
        public const byte BUSY = 255;
        public const byte NOPROG = 254;
        public const byte FINISHED = 253;
	
        private TcpClient client;
		private Virtual_Machine VM = Virtual_Machine.Instance;
		private NetworkStream clientStream;
		private Thread programThread;

		private bool keepLooping = true;
        private bool programLoaded = false;
        private bool paused = false;
        private bool connected = false;
        private bool shutdown = false;
		//private int programStatus = NO_PROGRAM_RESPONSE;

        public Request_Handler(TcpClient client) {
            this.client = client;
			clientStream = client.GetStream();
        }

        public void processRequest() {
            //Grab a handle to the virtual machine
            Virtual_Machine VM = Virtual_Machine.Instance;
            NetworkStream clientStream = client.GetStream();
            Console.Write("Connection request pending... \n");

            #region Try
            try {
            #endregion

                //Lock VM form use
                if (!VM.lockVM()) {
                    Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection refused, Virtual Machine busy");
                    Console.Write("Connection rejected: VM busy \n");
                    clientStream.WriteByte(BUSY);
                } else {
                    Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection Accepted");
                    Console.Write("Connection accepted \n");
                    clientStream.WriteByte(ACK);
                    connected = true;
                }

                while (connected) {
                    keepLooping = true;
                    programLoaded = false;
                    paused = false;

                    //Accept program load request
                    while (programLoaded == false) {
                        if (clientStream.DataAvailable) {
                            int message = clientStream.ReadByte();

                            if (message == DISCON) {
                                Console.Write("Server: discon recieved \n");
                                //Disconnect request, set comm loop to not run then break.
                                disconnect();
                                clientStream.WriteByte(ACK);
                                break;

                            } else if (message != SENDPROG) {
                                clientStream.WriteByte(NOPROG);
                            } else {
                                programLoaded = true;
                                clientStream.WriteByte(SENDPROG);

                                //Deserialise the workspace object from the client stream
                                StartBlock program = StartBlock.Deserialise(clientStream);
                                Console.Write("Program recieved \n");
                                Protocol lynxProtocol = new RobotInitial.LynxProtocol.LynxProtocol();

                                //Load the program onto the virtual machine then spawn another thread so we can 
                                //continue revieving communication from the client.
                                VM.LoadProgram(program, lynxProtocol);
                                programThread = new Thread(VM.RunProgram);
                                programThread.Start();
                                clientStream.WriteByte(ACK);
                            }
                        }
                    } //END Program Load

                    while (keepLooping) {

                        //DO we have any communications from the client to be processed
                        if (clientStream.DataAvailable) {
                            int message = clientStream.ReadByte();

                            if (message == STOP) {

                                Console.Write("Stopping Program \n");
                                VM.TerminateProgram(Shutdown.Software);
                                clientStream.WriteByte(ACK);
                                VM.Reset();
                                break;

                            } else if (message == PAUSE) {

                                Console.Write("Pausing Program \n");
                                VM.pause();
                                paused = true;
                                clientStream.WriteByte(ACK);

                            } else if (message == RESUME) {

                                Console.Write("Resuming Program \n");
                                VM.resume();
                                paused = false;
                                clientStream.WriteByte(ACK);

                            } else if (message == STATUS) {
                                if (paused) {
                                    clientStream.WriteByte(PAUSE);
                                } else if (VM.state == EndState.None) {
                                    clientStream.WriteByte(ACK);
                                } else if (VM.state == EndState.Completed) {
                                    clientStream.WriteByte(FINISHED);
                                }
                            } else if (message == DISCON) {

                                Console.Write("Disconnecting from GUI Shutting down program \n");
                                VM.TerminateProgram(Shutdown.Software);
                                disconnect();
                                clientStream.WriteByte(ACK);
                                break;

                            } else {
                                //If anything else say no.
                                clientStream.WriteByte(BUSY);
                            }
                        }

                        //Check if the thread is till running.
                        if (!programThread.IsAlive && !shutdown) {
                            Console.Write("program dead \n");
                            VM.Reset();
                            if (VM.state == EndState.Completed) {   
                                //Wait for next message from GUI reply program finished.
                                while (true) {
                                    if (clientStream.DataAvailable) {
                                        //Keep sending FINISHED until we get a status check then exit.
                                        int message = clientStream.ReadByte();
                                        clientStream.WriteByte(FINISHED);

                                        if (message == STATUS) {
                                            break;
                                        } else if (message == DISCON) {
                                            disconnect();
                                            break;
                                        }
                                    }
                                }

                            }

                            keepLooping = false;
                        } //Program thread Alive


                    } // Comm loop


                } //While connected

            #region Try
            } catch (Exception e) {
                VM.TerminateProgram(Shutdown.Hardware);
                VM.release();

            } finally {
                client.Close();
            }
            #endregion
        }

        private void disconnect() {
            Console.Write("VM release \n");
            keepLooping = false;
            shutdown = true;
            connected = false;
            VM.release();
        }
    }
}