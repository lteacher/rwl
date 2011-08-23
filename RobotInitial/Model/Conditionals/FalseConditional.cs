using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class FalseConditional : Conditional<bool> {
        public bool evaluate(Protocol protocol) {
            return false;
        }
    }
}
