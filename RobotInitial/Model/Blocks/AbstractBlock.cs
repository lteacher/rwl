using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    abstract class AbstractBlock : Block {

        public Point Location { get; set; }
        public Block Next { get; set; }

        public abstract void perform(Protocol protocol);

        public virtual Block NextToPerform {
            get { return Next; }
        }
    }
}
