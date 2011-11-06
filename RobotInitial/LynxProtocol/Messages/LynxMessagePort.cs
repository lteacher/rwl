using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace RobotInitial.LynxProtocol {
    //singleton to prevent multiple instances trying to claim the COM1 port
    sealed class LynxMessagePort {
        #region Singleton Stuff

        private static readonly LynxMessagePort instance = new LynxMessagePort();
        public static LynxMessagePort Instance {
            get { return instance; }
        }

        #endregion

        private const int TIMEOUT = 5000;
        private const String NEWLINE = "\r\n";
        private const String PORTNAME = "COM1";             //default "COM1"
        private const int BAUDRATE = 115200;                //default 9600
        private const Parity PARITY = Parity.None;          //default Parity.None
        private const int DATABITS = 8;                     //default 8
        private const StopBits STOPBITS = StopBits.One;     //default StopBits.One
        private readonly SerialPort port;
        private const int DELAYAFTERSEND = 50;

        private LynxMessagePort() {
            port = new SerialPort(PORTNAME, BAUDRATE, PARITY, DATABITS, STOPBITS);
            port.NewLine = NEWLINE;
            port.ReadTimeout = TIMEOUT;
        }

        ~LynxMessagePort() {
            ReleaseComPort();
        }

        public void ReleaseComPort() {
            port.Close();
            Console.WriteLine("Closed COM port");
        }

        public void ClaimComPort() {
            if (this.HasComPort()) {
                //if this is thrown then the logic of the server/vm is screwed and we want to know about it
                throw new ComPortAlreadyClaimedException();
            }

                Console.WriteLine("Openning COM port");
                port.Open();    //catch exception outside
        }

        public bool HasComPort() {
            return port.IsOpen;
        }

        //TODO: implement reliability (use checksum)
        public LynxMessage Send(LynxMessage m, bool isRequest) {
            if (!port.IsOpen) {
                throw new ComPortHasNotBeenClaimedException();
            }

            LynxMessage response;
            lock (this) {
                Console.WriteLine("Sending " + m.ToString() + " TO " + PORTNAME);
                port.WriteLine(m.ToString());

                if (isRequest) {
                    Console.WriteLine("Reading Response...");

                    //blocking call, timeout will be caught by the executor
                    //because we can't really do much if the COM port is not responding
                    //so the best thing to do is just kill the program
                    response = new LynxMessage(port.ReadLine());    

                    Console.WriteLine(response.ToString());
                } else {
                    Thread.Sleep(DELAYAFTERSEND);   //give the robot time to accept the message
                    response = null;
                }
            }
            return response;
        }
    }
}
