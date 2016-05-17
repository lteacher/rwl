using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using RobotInitial.Model;
using System.Threading;
using System.Net;

namespace RobotInitial.Lynx_Server {
    class Request_Handler {
<<<<<<< HEAD
        
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
	
=======
		public const byte OK_RESPONSE = 1;
		public const byte BUSY_RESPONSE = 0;
		public const byte COMPLETED_RESPONSE = 255;
		public const byte STOPPED_RESPONSE = 10;
		public const byte NO_PROGRAM_RESPONSE = 254;
		public const byte PROGRAM_EXECUTING_RESPONSE = 253;
		public const byte PROGRAM_PAUSED_RESPONSE = 252;

		public const int STOP_REQUEST = 83;
		public const int PAUSE_REQUEST = 80;
		public const int RESUME_REQUEST = 82;
		public const int PING_REQUEST = 98;
		public const int EXECUTE_REQUEST = 99;
		public const int DISCONNECT_REQUEST = 97;
        public const int PROGRAM_STATUS_REQUEST = 96;

>>>>>>> PreMerge
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

        public Request_Handler() { }

<<<<<<< HEAD
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
=======
		// Process the client request
        public void processRequest()
        {
			keepLooping = true;

			// Keep listening till told otherwise!
			while (keepLooping) {

				// if data available (should be if this method is called, possible exception?)
				if (clientStream.DataAvailable) {
					// get the message
					int message = clientStream.ReadByte();

					switch (message) {
						case PING_REQUEST:		// Return a status
							sendPingResult();
							break;
						case EXECUTE_REQUEST:	// Process a program
							processInboundProgram();
							break;
						case STOP_REQUEST:		// Stop a program that is running
							stopCurrentProgram();
							break;
						case PAUSE_REQUEST:		// Pause the currently executing program
							pauseCurrentProgram();
							break;
						case RESUME_REQUEST:	// Resume a paused program
							resumeCurrentProgram();
							break;
						case DISCONNECT_REQUEST:	// Disconnect, stop looping!
							closeConnection();
							break;
                        case PROGRAM_STATUS_REQUEST:	// Status request
							sendProgramStatus();
							break;
					}
				}

				// If the program thread is null lets skip it
				if(programThread == null) continue;

				// Otherwise lets check if its finished
				if (!programThread.IsAlive) {
					// If the VM is still in use, check its state
					if(!VM.isInitial()) {
						if (VM.state == EndState.Completed) {
							// update the current status
							programStatus = COMPLETED_RESPONSE;
						}
						else if (VM.state == EndState.TerminatedByClient) {
							//Confirm program termination
							programStatus = STOPPED_RESPONSE;
						}
						
						// Reset the VM
						try {
							VM.Reset();
						}
						catch (VirtualMachineOwnershipException) {
							Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Cannot release VM: Ownership Exception");
						}
					}
				}
			}
		}

        public void setClient(TcpClient client)
        {
            this.client = client;
            clientStream = client.GetStream();
        }

		public void closeConnection() {
			client.Close();
			clientStream.Close();
			keepLooping = false;
			Console.Write("Closed Connection \n");
			return;
		}

		// Send back the current status
		private void sendPingResult() {
			Console.WriteLine("Ping Request Accepted, Sending Response");
			if (VM.isInitial()) {
				clientStream.WriteByte(OK_RESPONSE);
			}
			else {
				clientStream.WriteByte(BUSY_RESPONSE);
			}
		}

		// Process a program that is ready to run
        private void processInboundProgram()
        {
			// Send a busy response if the VM is in use
			if (!VM.isInitial()) {
				clientStream.WriteByte(BUSY_RESPONSE);
				Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection refused, Virtual Machine busy");
				client.Close();
				return;
			}
			// Write an accepted message back
			clientStream.WriteByte(OK_RESPONSE);
			Console.Write("Execution Request Accepted \n");

			// Initialise the VM
			VM.Initialise();

			//Deserialise the workspace object from the client stream
			StartBlock program = StartBlock.Deserialise(clientStream);
			Console.Write("Program Recieved \n");
			Protocol lynxProtocol = new RobotInitial.LynxProtocol.LynxProtocol();

			//Load the program onto the virtual machine and start it on a new thread
			VM.LoadProgram(program, lynxProtocol);
			programThread = new Thread(VM.RunProgram);
			programThread.Start();
			programStatus = PROGRAM_EXECUTING_RESPONSE;			
		}

		private void stopCurrentProgram() {
			// Make sure there really is a running program
			if (programThread != null) {
				if (programThread.IsAlive) {
					Console.Write("Stopping Program \n");
					// Tell program to stop
					VM.TerminateProgram(Shutdown.Software);
				}
			}
		}

		private void pauseCurrentProgram() {
			// Make sure there really is a running program
			if (programThread != null) {
				if (programThread.IsAlive) {
					Console.Write("Pausing Program \n");
					// Tell program to pause
					VM.pause();
					programStatus = PROGRAM_PAUSED_RESPONSE;
				}
			}
		}

		private void resumeCurrentProgram() {
			// Make sure there really is a running program
			if (programThread != null) {
				if (programThread.IsAlive) {
					Console.Write("Resuming Program \n");
					// Tell program to resume
					VM.resume();
					programStatus = PROGRAM_EXECUTING_RESPONSE;
				}
			}
		}

		// Send the status of the program back to the caller
		private void sendProgramStatus() {
            clientStream.WriteByte((byte)programStatus);
		}

		//public void processRequest() {
		//    //Grab a handle to the virtual machine
		//    Virtual_Machine VM = Virtual_Machine.Instance;
		//    NetworkStream clientStream = client.GetStream();

		//    if (!VM.Initialise()) {
		//        //Reject connection, virtual machine is busy
		//        Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection refused, Virtual Machine busy");
		//        Console.Write("Connection rejected: VM busy \n");
		//        clientStream.WriteByte(BUSY_RESPONSE);
		//        client.Close();
		//        return;

		//    }
		//    else {
		//        //Virtual machine is free and has been allocated to this request
		//        Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Client connection Accepted");
		//        Console.Write("Connection accepted \n");
		//        clientStream.WriteByte(OK_RESPONSE);

		//        //Deserialise the workspace object from the client stream
		//        StartBlock program = StartBlock.Deserialise(clientStream);
		//        Console.Write("Program recieved \n");
		//        Protocol lynxProtocol = new RobotInitial.LynxProtocol.LynxProtocol();

		//        //Load the program onto the virtual machine then spawn another thread so we can 
		//        //continue revieving communication from the client.
		//        VM.LoadProgram(program, lynxProtocol);
		//        Thread programThread = new Thread(VM.RunProgram);
		//        programThread.Start();


		//        Boolean keepLooping = true;
		//        while (keepLooping) {
		//            if (clientStream.DataAvailable) {
		//                //DO we have any communications from the client to be processed
		//                int message = clientStream.ReadByte();

		//                if (message == 80) {
		//                    //Pause = 80
		//                    VM.pause();
		//                }
		//                else if (message == 82) {
		//                    //Resume == 82
		//                    VM.resume();
		//                }
		//                else if (message == 83) {
		//                    //Stop == 83
		//                    VM.TerminateProgram(Shutdown.Software);

		//                    //programThread.Abort();
		//                }

		//            }
		//            else if (!programThread.IsAlive) {
		//                //If the VM is not running, check why
		//                if (VM.state == EndState.Completed) {
		//                    //Send program completed to client
		//                    clientStream.WriteByte(255);
		//                }
		//                else if (VM.state == EndState.TerminatedByClient) {
		//                    //Confirm program termination
		//                    clientStream.WriteByte(10);
		//                }

		//                //Reset the VM so another program can run and exit request thread
		//                try {
		//                    VM.Reset();
		//                }
		//                catch (VirtualMachineOwnershipException) {
		//                    Lynx_Server.Log(DateTime.Now + " " + Lynx_Server.getIPAddress(client) + " Cannot release VM: Ownership Exception");
		//                }

		//                keepLooping = false;
		//            }

		//        }

		//        client.Close();
		//        return;
		//    }
		//}
>>>>>>> PreMerge

        private void disconnect() {
            Console.Write("VM release \n");
            keepLooping = false;
            shutdown = true;
            connected = false;
            VM.release();
        }
    }
}