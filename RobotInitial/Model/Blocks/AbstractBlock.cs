using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    [Serializable()]
    abstract class AbstractBlock : Block {

        public Point Location { get; set; }
        public Block Next { get; set; }

        public AbstractBlock() {
        }

        protected AbstractBlock(AbstractBlock other) {
            this.Location = other.Location;
            this.Next = (other.Next == null) ? null : other.Next.Clone() as Block;
        }

        public abstract Object Clone();

        public abstract void perform(Protocol protocol, ref LinkedList<Block> performAfter);
    }
}
