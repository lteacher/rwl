using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.IO;

namespace RobotInitial.Lynx_Server {
    #region LynxProtocol
    class LynxProtocol : Protocol {

        enum LynxCommand : byte {
            LFU = 1,
            LF = 2,
            LBU = 3,
            LB = 4,
            RFU = 5,
            RF = 6,
            RBU = 7,
            RB = 8,
            REQSTATUS = 128,
            REQIR = 129,
            REQIMU = 130,
            CMDCOMPLETE = 255
        }

        private const String MSGSTART = "$";
        private const String MSGEND = "\r\n";
        private const String MSGSEPERATOR = ",";
        private const int LEGOCOMMANDSET = 4;
        private const int ENCODERCOUNTSPERREVOLUTION = 500;

        public LynxProtocol() {
        }

        private String createMsg(params byte[] args) {
            StringBuilder msg = new StringBuilder();
            byte length = (byte)(args.Length + 1);      //length includes command set number

            msg.Append(MSGSTART);
            msg.Append(length).Append(MSGSEPERATOR);
            msg.Append(LEGOCOMMANDSET).Append(MSGSEPERATOR);

            byte checksum = (byte)(length ^ LEGOCOMMANDSET);    //"checksum is a logical XOR of each piece of information, including the length"

            foreach (byte arg in args) {
                checksum ^= arg;
                msg.Append(arg).Append(MSGSEPERATOR);
            }

            msg.Append(checksum);
            msg.Append(MSGEND);

            return msg.ToString();
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

        private LynxCommand getLeftCommand(MoveParameters parameters) {
            switch (parameters.LeftDirection) {
                case MoveDirection.FORWARD:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.LFU : LynxCommand.LF;
                case MoveDirection.BACK:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.LBU : LynxCommand.LB;
                case MoveDirection.STOP:
                default:
                    return LynxCommand.LF;  //no stop command....?
            }
        }

        private LynxCommand getRightCommand(MoveParameters parameters) {
            switch (parameters.RightDirection) {
                case MoveDirection.FORWARD:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.RFU : LynxCommand.RF;
                case MoveDirection.BACK:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.RBU : LynxCommand.RB;
                case MoveDirection.STOP:
                default:
                    return LynxCommand.RF;  //no stop command....?
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

            args[0] = (byte)getLeftCommand(parameters);
            args[1] = (byte)parameters.LeftPower;
            String leftMsg = createMsg(args);

            args[0] = (byte)getRightCommand(parameters);
            args[1] = (byte)parameters.RightPower;
            String rightMsg = createMsg(args);

            System.Diagnostics.Debug.Write(leftMsg);
            System.Diagnostics.Debug.Write(rightMsg);

            //throw new NotImplementedException();
        }

        public IRData requestIR() {
            String req = createMsg((byte)LynxCommand.REQIR);
            System.Diagnostics.Debug.Write(req);

            throw new NotImplementedException();
        }

        public IMUData requestIMU() {
            String req = createMsg((byte)LynxCommand.REQIMU);
            System.Diagnostics.Debug.Write(req);

            throw new NotImplementedException();
        }

        public Status requestStatus() {
            String req = createMsg((byte)LynxCommand.REQSTATUS);
            System.Diagnostics.Debug.Write(req);

            throw new NotImplementedException();
        }
    }
    #endregion

    #region LynxIRData
    struct LynxIRData : IRData {

        enum Port {
            FRONT,
            FRONTRIGHT,
            REARRIGHT,
            REAR,
            REARLEFT,
            FRONTLEFT
        }

        private const int NUMOFPORTS = 6;
        int[] distance;
        
        public LynxIRData(int front, int frontRight, int rearRight, int rear, int rearLeft, int frontLeft) {
            distance = new int[NUMOFPORTS];

            distance[(int)Port.FRONT] = front;
            distance[(int)Port.FRONTRIGHT] = frontRight;
            distance[(int)Port.REARRIGHT] = rearRight;
            distance[(int)Port.REAR] = rear;
            distance[(int)Port.REARLEFT] = rearLeft;
            distance[(int)Port.FRONTLEFT] = frontLeft;
        }

        public int getDistance(int port) {
            if (port > NUMOFPORTS - 1 || port < 0) {
                return -1;   //probaly should throw an exception or something, but the server wouldn't handle it atm
            }
            return distance[port];
        }
    }
    #endregion

    #region LynxIMUData
    struct LynxIMUData : IMUData {

        //structs cant have static inilizers
        private int[] accel;
        private int[] gyro;
        private int[] mag;

        public LynxIMUData(int accelX, int accelY, int accelZ, int gyroX, int gyroY, int gyroZ, int magX, int magY, int magZ) {
            accel = new int[3];
            gyro = new int[3];
            mag = new int[3];

            accel[(int)Axis.X] = accelX;
            accel[(int)Axis.Y] = accelY;
            accel[(int)Axis.Z] = accelZ;
            gyro[(int)Axis.X] = gyroX;
            gyro[(int)Axis.Y] = gyroY;
            gyro[(int)Axis.Z] = gyroZ;
            mag[(int)Axis.X] = magX;
            mag[(int)Axis.Y] = magY;
            mag[(int)Axis.Z] = magZ;
        }

        public int getAccelerometer(Axis axis) {
            return accel[(int)axis];
        }

        public int getGyroscope(Axis axis) {
            return gyro[(int)axis];
        }

        public int getMagnetometer(Axis axis) {
            return mag[(int)axis];
        }
    }
#endregion
}
