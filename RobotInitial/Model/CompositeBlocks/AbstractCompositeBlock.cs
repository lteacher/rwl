using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    [Serializable()]
    //quite pointless atm... this isn't serving its intended role, 
    //everything had to change due unforeseen situations and now this isn't really used at all
    abstract class AbstractCompositeBlock : AbstractBlock, CompositeBlock {

        abstract public List<Block> Paths { get; }

        public AbstractCompositeBlock() {
        }

        protected AbstractCompositeBlock(AbstractCompositeBlock other) : base(other) {
        }
    }
}
