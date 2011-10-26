﻿using System;
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

        private const String NEWLINE = "\r\n";
        private const String PORTNAME = "COM1";             //default "COM1"
        private const int BAUDRATE = 115200;                //default 9600
        private const Parity PARITY = Parity.None;          //default Parity.None
        private const int DATABITS = 8;                     //default 8
        private const StopBits STOPBITS = StopBits.One;     //default StopBits.One
        private readonly SerialPort port;
        private const int DELAYAFTERSEND = 30;

        private LynxMessagePort() {
            port = new SerialPort(PORTNAME, BAUDRATE, PARITY, DATABITS, STOPBITS);
            port.NewLine = NEWLINE;
            port.Open();
        }

        ~LynxMessagePort() {
            port.Close();
        }

        //TODO: implement reliability (use checksum)
        public LynxMessage Send(LynxMessage m, bool isRequest) {
            LynxMessage response;
            lock (this) {
                Console.WriteLine("Sending " + m.ToString() + " TO " + PORTNAME);
                port.WriteLine(m.ToString());
                Thread.Sleep(DELAYAFTERSEND);   //give the robot time to accept the message
                if (isRequest) Console.WriteLine("Reading Response...");
                response = isRequest ? new LynxMessage(port.ReadLine()) : null;
                if (isRequest) Console.WriteLine(response.ToString());
            }
            return response;
        }
    }
}