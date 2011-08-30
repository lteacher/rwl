using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface CompositeBlock : Block {
        List<Block> Paths { get; }      //all the different execution branches inside this block
    }
}
