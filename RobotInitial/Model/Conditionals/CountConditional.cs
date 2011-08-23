using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class CountConditional : Conditional<bool> {
        private int current = 0;

        private int limit = 1;
        public int CountUntil {
            get { return limit; }
            set { limit = Math.Max(1, value); }
        }

        public bool evaluate(Protocol protocol) {
            ++current;
            current %= limit;
            return current == 0;
        }

    }
}
