using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RobotInitial.Model {
    [Serializable()]
    class WaitBlock : AbstractBlock {

        public Conditional<bool> WaitUntil { get; set; }
        public bool initilised = false;

        internal WaitBlock() {
        }

        protected WaitBlock(WaitBlock other)
            : base(other) {
            this.WaitUntil = (other.WaitUntil == null) ? null : other.WaitUntil.Clone() as Conditional<bool>;
        }

        public override void Perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            if (!initilised) {
                initilised = true;
                WaitUntil.Initilize();
            }

            if (WaitUntil.Evaluate(protocol)) {
                //continue to next block
                initilised = false;
                performAfter.AddFirst(Next);
            } else {
                //condition not true yet, evalutate again
                //the block is exited so the stop/pause checks can occur while waiting
                performAfter.AddFirst(this);
            }
        }

        public override object Clone() {
            return new WaitBlock(this);
        }

        public override string ToString() {
            string s = "WaitUntil (" + this.WaitUntil + ")\n";
            return s + this.Next; 
        }
    }
}
