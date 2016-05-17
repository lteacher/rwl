using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    [Flags]
    enum LynxStatus : ushort {
        ERROR_WHEEL_SLIP = 0x1,
        ERROR_OVER_CURRENT = 0x2,
        ERROR_OVER_VOLTAGE = 0x4,
        ERROR_UNDER_VOLTAGE = 0x8,
        ERROR_NO_RESPONSE_MC = 0x10,
        NOT_READY = 0x20,
        COMMAND_INCOMPLETE = 0x40,
        ESTOP_ACTIVATED = 0x80,
        STATUS_CHECKSUM_FAIL = 0x100,
        NO_HEARTBEAT = 0x200
    }
}
