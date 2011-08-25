using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotInitial.Model {
    class WaitBrick : AbstractBrick {

        public Conditional<bool> WaitFor {
            set { Parameters["WaitFor"] = value; }
            get { return (Conditional<bool>)Parameters["WaitFor"]; }
        }

        public WaitBrick() {
            WaitFor = new TimeConditional();
        }

        public override void perform(Protocol protocol) {
            while (WaitFor.evaluate(protocol)) {
                Thread.Sleep(50);   //value may have to be adjusted
            }
        }
    }
}
