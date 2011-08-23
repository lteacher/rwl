using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class WaitBrick : AbstractBrick {

        public Conditional<bool> WaitFor {
            set { parameters["WaitFor"] = value; }
            get { return (Conditional<bool>)parameters["WaitFor"]; }
        }

        public WaitBrick() {
            WaitFor = new TimeConditional();
        }

        public override void perform(Protocol protocol) {
            while (WaitFor.evaluate(protocol)) { } //full busy wait probaly isn't ideal...

            //if (Next != null) {
            //    Next.perform(protocol);
            //}
        }
    }
}
