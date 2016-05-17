using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.LynxProtocol {
    enum PassCommand : byte {
        WRITE_CONTROL = 1,
        WRITE_RAW = 2,
        WRITE_FORWARD_ENC_CNT = 3,
        WRITE_FORWARD_REV_CNT = 4,
        WRITE_REVERSE_ENC_CNT = 5,
        WRITE_REVERSE_REV_CNT = 6,
        WRITE_RESET_ENC_CNT = 7,
        WRITE_RESET_REV_CNT = 8,
        WRITE_HEART_BEAT = 127,
        READ_ENCODER = 128,
        READ_RESET_ENCODER = 129,
        READ_REVOLUTIONS = 130,
        READ_RESET_REVOLUTIONS = 131,
        READ_RPM = 132,
        READ_DIRECTION = 135,
        READ_AMPS = 135,    //FIXME: SAME AS ABOVE? SAYS SO IN GIVEN SPEC SHEET
        READ_STATUS = 255
    }

    static class PassCommandExt {
        public static bool IsRequest(this PassCommand command) {
            return (byte)command >= 128;
        }
    }
}
