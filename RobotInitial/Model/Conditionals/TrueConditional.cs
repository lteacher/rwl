using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class TrueConditional : Conditional<bool> {
        public override bool evaluate(Protocol protocol) {
            return true;
        }

        public override void initilize() {
        }

        public override void update() {
        }
    }
}
