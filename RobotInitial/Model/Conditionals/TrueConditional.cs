using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class TrueConditional : Conditional<bool> {
        public bool evaluate(Protocol protocol) {
            return true;
        }
    }
}
