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

        protected WaitBlock(WaitBlock other)
            : base(other) {
            this.WaitUntil = (other.WaitUntil == null) ? null : other.WaitUntil.Clone() as Conditional<bool>;
        }

        public override void perform(Protocol protocol) {
            WaitUntil.initilize();
            while (!WaitUntil.evaluate(protocol)) {
                Thread.Sleep(50);   //value may have to be adjusted
            }
        }

        public override object Clone() {
            return new WaitBlock(this);
        }
    }
}
