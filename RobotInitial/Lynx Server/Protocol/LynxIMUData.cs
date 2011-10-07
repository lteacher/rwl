using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.Lynx_Server {
    struct LynxIMUData : IMUData {

        public Vector3 Accelerometer { get; private set; }
        public Vector3 Gyroscope { get; private set; }
        public Vector3 Magnetometer { get; private set; }

        public LynxIMUData(Vector3 accel, Vector3 gyro, Vector3 mag) : this() {
            Accelerometer = accel;
            Gyroscope = gyro;
            Magnetometer = mag;
        }
    }
}
