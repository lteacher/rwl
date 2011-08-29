using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    enum MoveParameter {
        DIRECTION,
        DURATION,
        DURATIONUNIT,
        POWER,
        STEERING,
        BRAKEAFTERMOVE
    }

    enum MoveDirection {
        FORWARD,
        STOP,
        BACK
    }

    enum MoveDurationUnit {
        ENCODERCOUNT,
        DEGREES,
        ROTATIONS,
        MILLISECONDS,
        UNLIMITED
    }

    interface MoveParameters {
        MoveDirection Direction { get; }
        MoveDurationUnit DurationUnit { get; }
        int Duration { get; }
        int Power { get; }
        int Steering { get; }
        bool BrakeAfterMove { get; }
    }
}
