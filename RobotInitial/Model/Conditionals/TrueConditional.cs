using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class TrueConditional : Conditional<bool> {
        public override bool Evaluate(Protocol protocol) {
            return true;
        }

        public override void Initilize() {
        }

        public override void Update() {
        }

        public override string ToString() {
            return "True";
        }
    }
}
