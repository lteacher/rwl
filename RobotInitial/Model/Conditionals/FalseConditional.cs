using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class FalseConditional : Conditional<bool> {
        internal FalseConditional() {
        }

        public override bool Evaluate(Protocol protocol) {
            return false;
        }

        public override void Initilize() {
        }

        public override void Update() {
        }

        public override string ToString() {
            return "False";
        }
    }
}
