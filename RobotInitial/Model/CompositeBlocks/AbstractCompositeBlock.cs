using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    abstract class AbstractCompositeBlock : AbstractBlock, CompositeBlock {

        abstract public List<Block> Paths { get; }
        abstract public Block InnerPathToPerform { get; }
        abstract override public Block NextToPerform { get; }

        public AbstractCompositeBlock() {
        }

        protected AbstractCompositeBlock(AbstractCompositeBlock other) : base(other) {
        }
    }
}
