using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    class LynxMessageFactory : MessageFactory {

        #region Constants

        //private static readonly LynxMessage BRAKEL = new LynxMessage((byte)LynxCommandSet.LEFTPASS, (byte)PassCommand.WRITE_RAW, BRAKEVAL, 0, 0);
        //private static readonly LynxMessage BRAKER = new LynxMessage((byte)LynxCommandSet.RIGHTPASS, (byte)PassCommand.WRITE_RAW, BRAKEVAL, 0, 0);

        //workaround since the real brake command makes the robot spasm
        private static readonly LynxMessage BRAKEL = new LynxMessage((byte)LynxCommandSet.LEFTPASS, (byte)PassCommand.WRITE_FORWARD_ENC_CNT, MINCMPERSEC, 0, 1);   
        private static readonly LynxMessage BRAKER = new LynxMessage((byte)LynxCommandSet.RIGHTPASS, (byte)PassCommand.WRITE_FORWARD_ENC_CNT, MINCMPERSEC, 0, 1);
        private static readonly LynxMessage REQSTATL = new LynxMessage((byte)LynxCommandSet.LEFTPASS, (byte)PassCommand.READ_STATUS, 0, 0, 0);
        private static readonly LynxMessage REQSTATR = new LynxMessage((byte)LynxCommandSet.RIGHTPASS, (byte)PassCommand.READ_STATUS, 0, 0, 0);
        private static readonly LynxMessage REQIMU = new LynxMessage((byte)LynxCommandSet.BASE, (byte)BaseCommand.REQIMU, 0, 0, 0);
        private static readonly LynxMessage REQIR = new LynxMessage((byte)LynxCommandSet.BASE, (byte)BaseCommand.REQIR, 0, 0, 0);

        private const byte MINCMPERSEC = 10;
        private const byte MAXCMPERSEC = 126; 
        private const float MAXPOWER = 100.0f;
        private const int ENCODERPERREVOLUTION = 1600;   
        private const float WHEELCIRCUMFERENCECM = 37.9f;
        private const float ENCODERPERCM = ENCODERPERREVOLUTION / WHEELCIRCUMFERENCECM;  
        private const ushort CONTINUOUS = 0xFFFF;
        private const ushort NOTCONTINUOUS = 0xFFFE;
        private const int RAWBRAKEVAL = 127;
        private const int MINSCALE = 12;

        #endregion

        #region Continuous Message

        private byte CalcRawValue(int power, MoveDurationUnit durationUnit, MoveDirection direction) {
            int scaledPower = (byte)(RAWBRAKEVAL * (power / MAXPOWER));
            scaledPower = Math.Max(scaledPower, MINSCALE);
            if (direction == MoveDirection.FORWARD) {
                scaledPower = RAWBRAKEVAL + scaledPower;
            } else if (direction == MoveDirection.BACK) {
                scaledPower = RAWBRAKEVAL - scaledPower;
            }
            return (byte)scaledPower;
        }

        private LynxMessage CreateContinuousMsg(MoveParameters parameters, Side side) {
            byte set;
            const byte cmd = (byte)PassCommand.WRITE_RAW;
            byte value;

            if (side == Side.LEFT) {
                set = (byte)LynxCommandSet.LEFTPASS;
                value = CalcRawValue(parameters.LeftPower, parameters.DurationUnit, parameters.LeftDirection);
            } else {
                set = (byte)LynxCommandSet.RIGHTPASS;
                value = CalcRawValue(parameters.RightPower, parameters.DurationUnit, parameters.RightDirection);
            }

            //Console.WriteLine(set, 
            return new LynxMessage(set, cmd, value, 0, 0);
        }

        #endregion

        #region Pass Message

        private PassCommand CalcMoveCommand(MoveDirection direction) {
            return (direction == MoveDirection.BACK) ? PassCommand.WRITE_REVERSE_ENC_CNT : PassCommand.WRITE_FORWARD_ENC_CNT;
        }

        private byte CalcCmPerSec(int power) {
            return (byte)((power / MAXPOWER) * (MAXCMPERSEC - MINCMPERSEC) + MINCMPERSEC);
        }


        private ushort CalcEncoderCounts(MoveDurationUnit unit, float duration, byte speed) {
            //convert to encoder counts
            ushort encoder;
            switch (unit) {
                default:
                case MoveDurationUnit.ENCODERCOUNT:
                    encoder = (ushort)duration;
                    break;
                case MoveDurationUnit.DEGREES:
                    encoder = (ushort)((duration / 360.0f) * ENCODERPERREVOLUTION);
                    break;
                case MoveDurationUnit.MILLISECONDS:
                    //speed is in cm/s
                    encoder = (ushort)((speed * ENCODERPERCM) * (duration / 1000));
                    break;
                case MoveDurationUnit.UNLIMITED:
                    encoder = CONTINUOUS;
                    break;
            }

            if (encoder == CONTINUOUS && unit != MoveDurationUnit.UNLIMITED) {
                return NOTCONTINUOUS;
            } else {
                return encoder;
            }
        }


        private LynxMessage CreatePassMsg(MoveParameters parameters, Side side) {
            LynxCommandSet set;
            PassCommand command;
            byte speed;
            ushort duration;

            if (side == Side.LEFT) {
                set = LynxCommandSet.LEFTPASS;
                command = CalcMoveCommand(parameters.LeftDirection);
                speed = CalcCmPerSec(parameters.LeftPower);
                duration = CalcEncoderCounts(parameters.DurationUnit, parameters.LeftDuration, speed);
            } else {
                set = LynxCommandSet.RIGHTPASS;
                command = CalcMoveCommand(parameters.RightDirection);
                speed = CalcCmPerSec(parameters.RightPower);
                duration = CalcEncoderCounts(parameters.DurationUnit, parameters.RightDuration, speed);
            }
            return new LynxMessage((byte)set, (byte)command, speed, (byte)((duration & 0xFF00) >> 8), (byte)(duration & 0x00FF));
        }

        #endregion

        #region MessageFactory Implementation

        public LynxMessage CreateMoveMsg(MoveParameters parameters, Side side) {
            MoveDirection dir = (side == Side.LEFT) ? parameters.LeftDirection : parameters.RightDirection;
            if (dir == MoveDirection.STOP) {
                return this.CreateBrakeMsg(side);
            }

            if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
                return this.CreateContinuousMsg(parameters, side);
            }

            return this.CreatePassMsg(parameters, side);

        }

        public LynxMessage CreateBrakeMsg(Side side) {
            return (Side.LEFT == side) ? BRAKEL : BRAKER;
        }

        public LynxMessage CreateStatusReq(Side side) {
            return (Side.LEFT == side) ? REQSTATL : REQSTATR;
        }

        public LynxMessage CreateIMUReq() {
            return REQIMU;
        }

        public LynxMessage CreateIRReq() {
            return REQIR;
        }

        #endregion

        /*test method
        public static void Main() {
            MoveBlock block = Model.DefaultBlockFactory.Instance.CreateMoveBlock();
            LynxMessageFactory fact = new LynxMessageFactory();
            block.DurationUnit = MoveDurationUnit.MILLISECONDS;
            block.LeftDuration = 1000;
            block.RightDuration = 500;
            Console.WriteLine(fact.CreateMoveMsg(block as MoveParameters, Side.LEFT));
            Console.WriteLine(fact.CreateMoveMsg(block as MoveParameters, Side.RIGHT));
            while (true) ;
        }*/
    }
}
