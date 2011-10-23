using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    enum LegoCommand : byte {
        LFU = 1,
        LF = 2,
        LBU = 3,
        LB = 4,
        RFU = 5,
        RF = 6,
        RBU = 7,
        RB = 8,
        REQLSTATUS = 128,
        REQRSTATUS = 129,
        REQIR = 130,
        REQIMU = 131,
        CMDCOMPLETE = 255
    }

    static class LegoCommandExt {
        public static LegoCommand GetLeftCommand(MoveParameters parameters) {
            switch (parameters.LeftDirection) {
                case MoveDirection.FORWARD:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LegoCommand.LFU : LegoCommand.LF;
                case MoveDirection.BACK:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LegoCommand.LBU : LegoCommand.LB;
                case MoveDirection.STOP:
                default:
                    return LegoCommand.LF;  //no stop command....?
            }
        }

        public static LegoCommand GetRightCommand(MoveParameters parameters) {
            switch (parameters.RightDirection) {
                case MoveDirection.FORWARD:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LegoCommand.RFU : LegoCommand.RF;
                case MoveDirection.BACK:
                    return parameters.DurationUnit == MoveDurationUnit.UNLIMITED ? LegoCommand.RBU : LegoCommand.RB;
                case MoveDirection.STOP:
                default:
                    return LegoCommand.RF; 
            }
        }
    }
}
