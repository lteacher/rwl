﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial {
    struct Vector3 {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z) : this() {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
