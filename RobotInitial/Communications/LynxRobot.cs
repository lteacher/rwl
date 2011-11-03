using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RobotInitial.Model;

namespace RobotInitial.Communications {
    class LynxRobot {

        #region static stuff
        public static List<IPEndPoint> lynxAddresses = new List<IPEndPoint>() {            
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7331)
        };

        private static Boolean connected = false;

        public static LynxRobot connectToRobotNumber(int robotNumber) {
            if (connected) {
                throw new AlreadyConnectedException();
            } else {
                connected = true;
            }

            TcpClient client = new TcpClient();
            client.Connect(lynxAddresses[robotNumber]);
            NetworkStream connection = client.GetStream();

            //Read response from server. 1 = ready, 0 = busy.
            int response = connection.ReadByte();
            Console.Write("Response recieved: " + response + "\n");

            if (response == 255) {
                throw new RobotInitial.LynxBusyException();                
            } else {                
                return new LynxRobot(connection);
            }
        }
        #endregion


        private NetworkStream connection;
        private Boolean programLoaded = false;
        private Boolean programPaused = false;

        private LynxRobot(NetworkStream connection) {
            this.connection = connection;
        }

        public void setProgram(StartBlock program){
            //Request to send program
            connection.WriteByte(0);
            int response = connection.ReadByte();

            if (response == 255) {
                throw new RobotInitial.LynxBusyException();
            } else {
                program.Serialise(connection);
                Console.Write("Program sent \n");

                //Wait for complete response from robot. 
                response = connection.ReadByte();
                programLoaded = true;
            }
        }

        public void pauseProgram() {
            if (!programLoaded) {
                throw new NotConnectedException();
            } else if (!programPaused) {
                connection.WriteByte(82);
                int response = connection.ReadByte();

                if (response == 255) {
                    throw new RobotInitial.LynxBusyException();
                }

                Console.Write("Program paused \n");
            }
        }

        public void resumeProgram() {
            if (!programLoaded) {
                throw new NotConnectedException();
            } else if (!programPaused) {
                throw new LynxNotPausedException();
            } else {
                connection.WriteByte(82);
                int response = connection.ReadByte();

                if (response == 255) {
                    throw new RobotInitial.LynxBusyException();
                }

                Console.Write("Program resumed \n");
            }
        }

        public void stopProgram() {
            if (!programLoaded) {
                throw new NotConnectedException();
            } else {
                connection.WriteByte(83);
                int response = connection.ReadByte();

                if (response == 255) {
                    throw new RobotInitial.LynxBusyException();
                }

                Console.Write("Program stopped \n");
            }
        }
        


    }
}
