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

namespace LynxTest2.Communications {
    class Network {

        public static List<IPEndPoint> lynxAddresses = new List<IPEndPoint>() {            
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7331)
        };

         
        public static void connectToLynx(IPEndPoint robot, StartBlock program){
            TcpClient client = new TcpClient();
            client.Connect(robot);
            NetworkStream clientStream = client.GetStream();

            //Read response from server. 1 = ready, 0 = busy.
            int response = clientStream.ReadByte();
            Console.Write("Response recieved: " + response + "\n");

            if (response == 1) {
                program.Serialise(clientStream);
                Console.Write("Program sent \n");
            } else {
                throw new RobotInitial.LynxBusyException();
            }

        }

        public static void send(MemoryStream item, Stream stream) {
            //Byte buffer to hold item to be sent.
            byte[] message = item.GetBuffer();

            //Byte buffer to hold length of item message.
            byte[] messageLength = BitConverter.GetBytes(message.Length);

            //Send length of incomming item to server. 4 = #bytes in int.
            stream.Write(messageLength, 0, 4);
            
            //Finally send the message.
            stream.Write(message, 0, message.Length);
        }

        public static MemoryStream recieve(Stream stream) {
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
