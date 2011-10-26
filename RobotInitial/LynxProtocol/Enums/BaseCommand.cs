using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    enum BaseCommand : byte {
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
}
