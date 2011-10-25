using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotInitial.Model {
    [Serializable()]
    class WaitBlock : AbstractBlock {

        public Conditional<bool> WaitUntil { get; set; }

        internal WaitBlock() {
        }

        protected WaitBlock(WaitBlock other)
            : base(other) {
            this.WaitUntil = (other.WaitUntil == null) ? null : other.WaitUntil.Clone() as Conditional<bool>;
        }

        public override void Perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            performAfter.AddFirst(Next);

            WaitUntil.Initilize();
            while (!WaitUntil.Evaluate(protocol)) {
                //sleeping may potentially reduce the reponsiveness of the conditional which could
                //obviously cause problems.... ie the robot crashing into the wall
                //because the IR sensor wasn't polled often enough. But I don't really know if this would be an issue. 
                //This just has to be tested.
                //Thread.Sleep(10);
            }
        }

        public override object Clone() {
            return new WaitBlock(this);
        }
    }
}
