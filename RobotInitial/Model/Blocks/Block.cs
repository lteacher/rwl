using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RobotInitial.Model {
    interface Block {
        Point Location { get; set; }
        Block Next { get; set; }            //link to the next block
        void perform(Protocol protocol);    //executes the block, updates NextToPerform
        Block NextToPerform { get; }        //The block that should be executed after this has finished
    }
}
