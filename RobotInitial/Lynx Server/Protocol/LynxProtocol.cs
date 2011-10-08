using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.IO;
using System.IO.Ports;

namespace RobotInitial.Lynx_Server {

    class LynxProtocol : Protocol {
        private const int ENCODERCOUNTSPERREVOLUTION = 500;

        private const String NEWLINE = "\r\n";
        private const String PORTNAME = "COM1";             //default "COM1"
        private const int BAUDRATE = 9600;                  //default 9600
        private const Parity PARITY = Parity.None;          //default Parity.None
        private const int DATABITS = 8;                     //default 8
        private const StopBits STOPBITS = StopBits.One;     //default StopBits.One
        private readonly SerialPort port;

        public LynxProtocol() {
            port = new SerialPort(PORTNAME, BAUDRATE, PARITY, DATABITS, STOPBITS);
            port.NewLine = NEWLINE;
            port.Open();
        }

        ~LynxProtocol() {
            port.Close();
        }

        
        private void sendMessage(LynxMessage message) {
            port.WriteLine(message.ToString());
        }

        //FIXME: possibly too simplistic, probably will have to do some error checking and such
        private LynxMessage getResponse() {
            return new LynxMessage(port.ReadLine());    //blocking call
        }

        private int duration2Encoder(MoveDurationUnit unit, float val) {
            switch (unit) {
                case MoveDurationUnit.ENCODERCOUNT:
                    return (int)val;
                case MoveDurationUnit.DEGREES:
                    return (int)((val / 360.0f) * ENCODERCOUNTSPERREVOLUTION);
                case MoveDurationUnit.UNLIMITED:
                default:
                case MoveDurationUnit.MILLISECONDS: //FIXME: may have to  manually time it....
                    return 0;
            }
        }

        public void move(MoveParameters parameters) {
            byte[] args;

            if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
                args = new byte[2];
            } else {
                args = new byte[4];
                int dur = duration2Encoder(parameters.DurationUnit, parameters.Duration);
                args[2] = (byte)((dur & 0xFF00) >> 8);  //higher byte
                args[3] = (byte)(dur & 0x00FF);         //lower byte
            }

            args[0] = (byte)LynxCommandExt.getLeftCommand(parameters);
            args[1] = (byte)parameters.LeftPower;
            LynxMessage leftMsg = new LynxMessage(args);

            args[0] = (byte)LynxCommandExt.getRightCommand(parameters);
            args[1] = (byte)parameters.RightPower;
            LynxMessage  rightMsg = new LynxMessage(args);

            sendMessage(leftMsg);
            sendMessage(rightMsg);
            getResponse();
            getResponse();//block until both complete
        }

        public IRData requestIR() {
            sendMessage(new LynxMessage((byte)LynxCommand.REQIR));
            LynxMessage resp = getResponse();

            int[] distances = new int[resp.Length];
            for (int i = 0; i < distances.Length; ++i) {
                distances[i] = resp.getArg(0);
            }
            return new LynxIRData(distances);
        }

        public IMUData requestIMU() {
            sendMessage(new LynxMessage((byte)LynxCommand.REQIMU));
            LynxMessage resp = getResponse();

            return new LynxIMUData(
                new Vector3(resp.getArg(0), resp.getArg(1), resp.getArg(2)),
                new Vector3(resp.getArg(3), resp.getArg(4), resp.getArg(5)),
                new Vector3(resp.getArg(6), resp.getArg(7), resp.getArg(8)));
        }

        public int requestStatus() {
            sendMessage(new LynxMessage((byte)LynxCommand.REQSTATUS));
            LynxMessage resp = getResponse();
            return (resp.getArg(0) << 8) | resp.getArg(1);
        }
    }
}
