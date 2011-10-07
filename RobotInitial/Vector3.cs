using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial {
    struct Vector3 {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Vector3(int x, int y, int z) : this() {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
