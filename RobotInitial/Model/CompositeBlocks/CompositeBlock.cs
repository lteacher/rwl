using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface CompositeBlock : Block {
        List<Block> Paths { get; }      //all the different execution branches inside this block
        Block InnerPathToPerform { get; }    //the inner path to be executed. To be updated during Block.Perform
    }
}
