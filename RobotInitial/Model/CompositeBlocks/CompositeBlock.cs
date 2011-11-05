using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //quite pointless atm... this isn't serving its intended role, 
    //everything had to change due unforeseen situations and now this isn't really used at all
    interface CompositeBlock : Block {
        List<Block> Paths { get; }      //all the different execution branches inside this block
    }
}
