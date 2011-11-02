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
		//public static List<IPEndPoint> lynxAddresses = new List<IPEndPoint>() {            
		//    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7331)
		//};

		public static readonly Network Instance = new Network();
		public static readonly int DefaultPort = 7331;
        private NetworkStream connection;
		private TcpClient client;
		private IPEndPoint connectedRobot;

		private Network() { /* Force non-instantiability */ } 

        // Initiate connection to a lynx robot 
		public void connectToLynx(IPEndPoint robot) {
			client = new TcpClient();
			connectedRobot = robot;
			client.Connect(robot);
			connection = Instance.client.GetStream();
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
			// Send a ping request
			connection.WriteByte(Request_Handler.PING_REQUEST);

			//Read response from server. 1 = ready, 0 = busy.
			int response = connection.ReadByte();

			// Close the connection
			closeConnection();

			return response == Request_Handler.OK_RESPONSE ? true : false;
		}

		// Close the connection to the Lynx
		public void closeConnection() {
			// Send a ping request
			connection.WriteByte(Request_Handler.DISCONNECT_REQUEST);

			//Read response from server. 1 = ready, 0 = busy.
			int response = connection.ReadByte();

			client.Close();
			connection.Close();
		}

		// Check if the connection is still active
		public bool isConnected() {
			return client.Connected;
		}

        public void stopProgram() {
            if (connection != null) {
                connection.WriteByte(Request_Handler.STOP_REQUEST);
            }
        }

        public void pauseProgram() {
            if (connection != null) {
                connection.WriteByte(80);
            }
        }

        public void resumeProgram() {
            if (connection != null) {
                connection.WriteByte(82);
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
