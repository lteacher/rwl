using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class StartBlock : AbstractBlock {

        public StartBlock() {
        }

        protected StartBlock(StartBlock other) : base(other) {
        }

        public override void perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            performAfter.AddFirst(Next);
        }

        public override object Clone() {
            return new StartBlock(this);
        }
    }
}
