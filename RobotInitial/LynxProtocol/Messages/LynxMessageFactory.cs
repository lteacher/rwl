using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    class LynxMessageFactory : MessageFactory {
        private static readonly LynxMessage BRAKEL = new LynxMessage((byte)LynxCommandSet.LEFTPASS, (byte)PassCommand.WRITE_FORWARD_ENC_CNT, 1, 0, 1);
        private static readonly LynxMessage BRAKER = new LynxMessage((byte)LynxCommandSet.RIGHTPASS, (byte)PassCommand.WRITE_FORWARD_ENC_CNT, 1, 0, 1);
        private static readonly LynxMessage REQSTATL = new LynxMessage((byte)LynxCommandSet.LEFTPASS, (byte)PassCommand.READ_STATUS, 0, 0, 0);
        private static readonly LynxMessage REQSTATR = new LynxMessage((byte)LynxCommandSet.RIGHTPASS, (byte)PassCommand.READ_STATUS, 0, 0, 0);
        private static readonly LynxMessage REQIMU = new LynxMessage((byte)LynxCommandSet.BASE, (byte)BaseCommand.REQIMU, 0, 0, 0);
        private static readonly LynxMessage REQIR = new LynxMessage((byte)LynxCommandSet.BASE, (byte)BaseCommand.REQIR, 0, 0, 0);

        //TODO: find out the correct values for the below constants.
        //The calculations will be inaccurate if these values are off.
        private const byte MINSPEED = 1;
        private const byte MAXSPEED = 255;  //CM/S, ARBITRARY VALUE
        private const float MAXPOWER = 100.0f;
        private const int ENCODERPERREVOLUTION = 500;   //ARBITRARY VALUE
        private const float ENCODERPERCM = 10;  //ARBITRARY VALUE
        private const ushort CONTINUOUS = 0xFFFF;
        private const ushort NOTCONTINUOUS = 0xFFFE;


        private PassCommand calcMoveCommand(MoveDirection direction) {
            switch (direction) {
                case MoveDirection.BACK:
                    return PassCommand.WRITE_REVERSE_ENC_CNT;
                case MoveDirection.FORWARD:
                case MoveDirection.STOP:
                default:
                    return PassCommand.WRITE_FORWARD_ENC_CNT;
            }
        }

        private byte calcSpeed(int power) {
            byte speed = (byte)((power / MAXPOWER) * MAXSPEED);
            return Math.Max(speed, MINSPEED);
        }

        private ushort calcEncoderCounts(MoveDurationUnit unit, float duration, byte speed) {
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
                    return CONTINUOUS;
            }

            return (encoder == CONTINUOUS) ? NOTCONTINUOUS : encoder;
        }

        public LynxMessage CreateMoveMsg(MoveParameters parameters, Side side) {
            MoveDirection dir = (side == Side.LEFT) ? parameters.LeftDirection : parameters.RightDirection;
            if (dir == MoveDirection.STOP) {
                return this.CreateBrakeMsg(side);
            }

            LynxCommandSet set;
            PassCommand command;
            byte speed;
            ushort duration;

            if (side == Side.LEFT) {
                set = LynxCommandSet.LEFTPASS;
                command = calcMoveCommand(parameters.LeftDirection);
                speed = calcSpeed(parameters.LeftPower);
                duration = calcEncoderCounts(parameters.DurationUnit, parameters.LeftDuration, speed);
            } else {
                set = LynxCommandSet.RIGHTPASS;
                command = calcMoveCommand(parameters.RightDirection);
                speed = calcSpeed(parameters.RightPower);
                duration = calcEncoderCounts(parameters.DurationUnit, parameters.RightDuration, speed);
            }
            return new LynxMessage((byte)set, (byte)command, speed, (byte)((duration & 0xFF00) >> 8), (byte)(duration & 0x00FF));
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

        /*
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
