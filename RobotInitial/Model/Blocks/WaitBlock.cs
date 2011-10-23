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
                Thread.Sleep(50);   //value may have to be adjusted
            }
        }

        public override object Clone() {
            return new WaitBlock(this);
        }
    }
}
