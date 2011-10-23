using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    class RawMessageFactory : LynxMessageFactory {
        private static readonly LynxMessage REQSTATL = new LynxMessage(LynxCommandSet.LEFTPASS, (byte)RawCommand.READ_STATUS);
        private static readonly LynxMessage REQSTATR = new LynxMessage(LynxCommandSet.RIGHTPASS, (byte)RawCommand.READ_STATUS);
        private static readonly LynxMessage REQIMU = new LynxMessage(LynxCommandSet.LEGO, (byte)LegoCommand.REQIMU);
        private static readonly LynxMessage REQIR = new LynxMessage(LynxCommandSet.LEGO, (byte)LegoCommand.REQIR);
        private static readonly LynxMessage BRAKEL = new LynxMessage(LynxCommandSet.LEFTPASS, (byte)RawCommand.WRITE_CONTROL, (byte)RawWriteControl.BRAKE, 0, 0);
        private static readonly LynxMessage BRAKER = new LynxMessage(LynxCommandSet.RIGHTPASS, (byte)RawCommand.WRITE_CONTROL, (byte)RawWriteControl.BRAKE, 0, 0);

        private static readonly int MAXSPEED = 255;  //cm/sec
        private static readonly float MAXPOWER = 100.0f;
        private const int ENCODERCOUNTSPERREVOLUTION = 500;

        private RawCommand getMoveCommand(MoveDirection direction, MoveDurationUnit unit) {
            /*may need to use REV command if ENC doesnt work for unlimited movement commands
             * if (unit == MoveDurationUnit.UNLIMITED) {
                switch (direction) {
                    case MoveDirection.BACK:
                        return RawCommand.WRITE_REVERSE_REV_CNT;
                    case MoveDirection.FORWARD:
                        return RawCommand.WRITE_FORWARD_REV_CNT;
                    case MoveDirection.STOP:
                    default:
                        return RawCommand.WRITE_CONTROL;
                }
            } else {*/
            switch (direction) {
                case MoveDirection.BACK:
                    return RawCommand.WRITE_REVERSE_ENC_CNT;
                case MoveDirection.FORWARD:
                    return RawCommand.WRITE_FORWARD_ENC_CNT;
                case MoveDirection.STOP:
                default:
                    return RawCommand.WRITE_CONTROL;
            }
            //}
        }

        private byte getSpeedOrControl(int power, MoveDirection direction) {
            if (direction == MoveDirection.STOP) {
                return (byte)RawWriteControl.BRAKE;
            }
            return (byte)((power / MAXPOWER) * MAXSPEED);
        }

        private ushort getDuration(MoveDurationUnit unit, float duration) {
            //convert to encoder counts
            switch (unit) {
                case MoveDurationUnit.ENCODERCOUNT:
                    return (ushort)duration;
                case MoveDurationUnit.DEGREES:
                    return (ushort)((duration / 360.0f) * ENCODERCOUNTSPERREVOLUTION);
                case MoveDurationUnit.UNLIMITED:
                case MoveDurationUnit.MILLISECONDS: //will have to time it in the protocol.
                default:
                    return 0xFFFF;
            }
        }

        public LynxMessage CreateMoveMsg(MoveParameters parameters, Side side) {
            LynxCommandSet set;
            RawCommand command;
            byte speed;
            ushort duration;

            if (side == Side.LEFT) {
                set = LynxCommandSet.LEFTPASS;
                command = getMoveCommand(parameters.LeftDirection, parameters.DurationUnit);
                speed = getSpeedOrControl(parameters.LeftPower, parameters.LeftDirection);
                duration = getDuration(parameters.DurationUnit, parameters.Duration);
            } else {
                set = LynxCommandSet.RIGHTPASS;
                command = getMoveCommand(parameters.RightDirection, parameters.DurationUnit);
                speed = getSpeedOrControl(parameters.RightPower, parameters.RightDirection);
                duration = getDuration(parameters.DurationUnit, parameters.Duration);
            }
            return new LynxMessage(set, (byte)command, speed, (byte)((duration & 0xFF00) >> 8), (byte)(duration & 0x00FF));
        }

        public LynxMessage CreateBrakeMsg(Side side) {
            return (Side.LEFT == side) ? BRAKEL : BRAKER;
        }

        public LynxMessage CreateStatusReq(Side side) {
            return (side == Side.LEFT) ? REQSTATL : REQSTATR;
        }

        public LynxMessage CreateIMUReq() {
            return REQIMU;
        }

        public LynxMessage CreateIRReq() {
            return REQIR;
        }
    }
}
