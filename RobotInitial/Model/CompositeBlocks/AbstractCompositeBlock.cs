using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    abstract class AbstractCompositeBlock : AbstractBlock, CompositeBlock {

        abstract public List<Block> Paths { get; }

        public AbstractCompositeBlock() {
        }

        protected AbstractCompositeBlock(AbstractCompositeBlock other) : base(other) {
        }
    }
}
