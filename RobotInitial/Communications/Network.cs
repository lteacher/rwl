using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using RobotInitial.Model;
using RobotInitial.Lynx_Server;
using RobotInitial;

namespace LynxTest2.Communications {
    class Network {
        public const int PING_TIMEOUT = 1000;
        public const int STANDARD_TIMEOUT = 4000;
		public static readonly Network Instance = new Network();
		public static readonly int DefaultPort = 7331;
        private NetworkStream connection;
		private TcpClient client;
		private IPEndPoint connectedRobot;
		private bool Connected = false;

		private Network() { /* Force non-instantiability */ } 

        // Initiate connection to a lynx robot 
		public void connectToLynx(IPEndPoint robot, int timeout) {
            connection = null;
			client = new TcpClient();
			connectedRobot = robot;
			// Connect Asynchonously
			IAsyncResult result = client.BeginConnect(robot.Address, DefaultPort, null, null);

			// Wait for success max timout duration
			bool success = result.AsyncWaitHandle.WaitOne(timeout);

			// If unsuccessful throw a SocketException
			if(!success) {
				// If the connnection is not null
				if(connection != null) {
					// Shut down the connection the HARD way since it should not be running
					connection.Close();
					client.Close();
					Connected = false;
					throw new SocketException();
				}
			}

			// Since Connected property is useless lets try catch
			try {
				connection = Instance.client.GetStream();
				Connected = true;
			} catch(InvalidOperationException exc) {
				Connected = false;
			}
		}

		// Send the program, called on button start press
		public void startProgram(StartBlock program) {
			// Send a Execution request
			connection.WriteByte(Request_Handler.EXECUTE_REQUEST);

			// Get the response
			int response = connection.ReadByte();

			Console.Write("Response recieved: " + response + "\n");

			if (response == Request_Handler.OK_RESPONSE) {
				program.Serialise(connection);
				Console.Write("Program sent \n");
			}
			if (response == Request_Handler.BUSY_RESPONSE) {
				throw new RobotInitial.LynxBusyException();
			}
		}

		// Check if a connection is available to a robot
		public bool robotConnectionAvail() {
			
			// Make sure previous connecting step succeeded
			if(!Connected) return false;

			// Send a ping request
			connection.WriteByte(Request_Handler.PING_REQUEST);

			// Create a buffer to take the asynchronous read
			byte[] buffer = new byte[1];

			//Read response asynchronously, will break on IO Block
			IAsyncResult result = connection.BeginRead(buffer,0,1,null,null);

			// Wait for success max timout duration
			bool success = result.AsyncWaitHandle.WaitOne(PING_TIMEOUT);

			// If unsuccessful throw a SocketException
			if (!success) {
				// If the connnection is not null
				if (connection != null) {
					// Shut down the connection the HARD way since it should not be running
					closeConnection();
					throw new SocketException();
				}
			}
			// Set the response
			int response = buffer[0];

			// Disconnect
			closeConnection();

			return response == Request_Handler.OK_RESPONSE ? true : false;
		}

		// Close the connection to the Lynx
		public void closeConnection() {
			if(Connected)connection.WriteByte(Request_Handler.DISCONNECT_REQUEST);
			connection.Close();
			client.Close();
			Connected = false;
		}

		// Check if the connection is still active
		public bool isConnected() {
			return Connected;
		}

        public int requestProgramStatus()
        {
			try {
				// Send a status request
				connection.WriteByte(Request_Handler.PROGRAM_STATUS_REQUEST);

				// Create a buffer to take the asynchronous read
				byte[] buffer = new byte[1];

				//Read response asynchronously, will break on IO Block
				IAsyncResult result = connection.BeginRead(buffer, 0, 1, null, null);

				// Wait for success max timout duration
				bool success = result.AsyncWaitHandle.WaitOne(PING_TIMEOUT);

				// If unsuccessful throw a SocketException
				if (!success) {
					// If the connnection is not null
					if (connection != null) {
						// Shut down the connection the HARD way since it should not be running
						closeConnection();
						//connection.Close();
						//client.Close();
						//Connected = false;
						throw new SocketException();
					}
				}

				// Set the response
				int response = buffer[0];

				// Return it
				return response;	
			} catch(IOException exc) {
				// If the connnection is not null
				if (connection != null) {
					// Shut down the connection the HARD way since it should not be running
					connection.Close();
					client.Close();
					Connected = false;
				}
				Console.WriteLine("Abnormal Termination due to IOException");
			}
			return -1;		
        }

        public void stopProgram() {
            if (connection != null) {
				try {
					if (Connected) connection.WriteByte(Request_Handler.STOP_REQUEST);
				}
				catch (IOException exc) {
					// Shut down the connection the HARD way since it should not be running
					connection.Close();
					client.Close();
					Connected = false;
					Console.WriteLine("Abnormal Termination due to IOException");
				}
            }
        }

        public void pauseProgram() {
            if (connection != null) {
				try {
					if (Connected) connection.WriteByte(Request_Handler.PAUSE_REQUEST);
				}
				catch (IOException exc) {
					// Shut down the connection the HARD way since it should not be running
					connection.Close();
					client.Close();
					Connected = false;
					Console.WriteLine("Abnormal Termination due to IOException");
				}
            }
        }

        public void resumeProgram() {
            if (connection != null) {
				try {
					if (isConnected()) connection.WriteByte(Request_Handler.RESUME_REQUEST);
				}
				catch (IOException exc) {
					// Shut down the connection the HARD way since it should not be running
					connection.Close();
					client.Close();
					Connected = false;
					Console.WriteLine("Abnormal Termination due to IOException");
				}
            }
        }

        public void send(MemoryStream item, Stream stream) {
            //Byte buffer to hold item to be sent.
            byte[] message = item.GetBuffer();

            //Byte buffer to hold length of item message.
            byte[] messageLength = BitConverter.GetBytes(message.Length);

            //Send length of incomming item to server. 4 = #bytes in int.
            stream.Write(messageLength, 0, 4);
            
            //Finally send the message.
            stream.Write(message, 0, message.Length);
        }

        public MemoryStream recieve(Stream stream) {
            MemoryStream ret = new MemoryStream();

            //Byte buffer to store incomming message length.
            byte[] messageLengthBuffer = new byte[4];

            //Counter to hold messge offset incase entire 4 bytes dont come though at once.
            int messageLengthRead = 0;

            //Read the 4 byte int length of the incomming message.
            while (messageLengthRead < 4) {
                messageLengthRead += stream.Read(messageLengthBuffer, messageLengthRead, (4 - messageLengthRead));
            }

            //Convert the byte buffer input to an int.
            int messageLength = BitConverter.ToInt32(messageLengthBuffer, 0);

            //Byte buffer to store message.
            byte[] messageBuffer = new byte[messageLength];

            //Counter to hold length of message read so far.
            int messageRead = 0;

            //Keep looping while entire message hasnt been read yet.
            while (messageRead < messageLength) {
                messageRead += stream.Read(messageBuffer, messageRead, (messageLength - messageRead));
            }

            //Move the data from the message buffer into the output memory stream.
            ret.Write(messageBuffer, 0, messageLength);
            ret.Position = 0;

            return ret;
        }

    }
}
