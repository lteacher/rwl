using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotInitial.Model {
    class WaitBlock : AbstractBlock {

        public Conditional<bool> WaitUntil { get; set; }

        public WaitBlock() {
            WaitUntil = new TimeConditional();
        }

        public override void perform(Protocol protocol) {
            WaitUntil.initilize();
            while (!WaitUntil.evaluate(protocol)) {
                Thread.Sleep(50);   //value may have to be adjusted
            }
        }
    }
}
