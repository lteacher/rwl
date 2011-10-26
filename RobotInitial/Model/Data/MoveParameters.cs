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
        //ROTATIONS,
        MILLISECONDS,
        UNLIMITED
    }

    interface MoveParameters {
        MoveDirection RightDirection { get; }
        MoveDirection LeftDirection { get; }
        int RightPower { get; }
        int LeftPower { get; }
        float RightDuration { get; }
        float LeftDuration { get; }
        MoveDurationUnit DurationUnit { get; }
        bool BrakeAfterMove { get; }
    }
}
