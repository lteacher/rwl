using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class FalseConditional : Conditional<bool> {
        internal FalseConditional() {
        }

        public override bool evaluate(Protocol protocol) {
            return false;
        }

        public override void initilize() {
        }

        public override void update() {
        }
    }
}
