using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    enum PassWriteControl : byte {
        COAST = 0,
        BRAKE = 1,
        RESET = 2,
        POWER_DOWN = 3,
        POWER_UP = 4,
        RELEASE_ESTOP = 5,
        RESET_STATUS = 6,
    }
}
