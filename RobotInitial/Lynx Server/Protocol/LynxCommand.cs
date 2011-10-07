using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.Lynx_Server {
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

    static class LynxCommandExt {
        public static LynxCommand getLeftCommand(MoveParameters parameters) {
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

        public static LynxCommand getRightCommand(MoveParameters parameters) {
            switch (parameters.RightDirection) {
                case MoveDirection.FORWARD:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.RFU : LynxCommand.RF;
                case MoveDirection.BACK:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LynxCommand.RBU : LynxCommand.RB;
                case MoveDirection.STOP:
                default:
                    return LynxCommand.RF; 
            }
        }
    }
}
