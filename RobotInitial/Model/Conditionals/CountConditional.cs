using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    class CountConditional : Conditional<bool> {
        private int count = 0;
        public int Limit { get; set; }

        internal CountConditional() {
        }

        public override void Initilize() {
            count = 0;
        }

        public override void Update() {
            ++count;
        }

        public override bool Evaluate(Protocol protocol) {
            //will evaulate true when loop is to terminate
            return count >= Limit;
        }
    }
}
