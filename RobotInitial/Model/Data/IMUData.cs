using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    enum Axis {
        X,
        Y,
        Z
    }

    interface IMUData {
        int getAccelerometer(Axis axis);
        int getGyroscope(Axis axis);
        int getMagnetometer(Axis axis);
    }
}
