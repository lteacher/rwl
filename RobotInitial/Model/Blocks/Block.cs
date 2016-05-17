using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    interface Block : ICloneable {
        Point Location { get; set; }
        Block Next { get; set; }                    //link to the next block
        void Perform(Protocol protocol, ref LinkedList<Block> performAfter);    
    }
}
