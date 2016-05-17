using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    enum LynxCommandSet : byte {
        LEFTPASS = 1,
        RIGHTPASS = 2,
        MAZE = 3,
        BASE = 4
    }

    static class LynxCommandExt {
    }
}
