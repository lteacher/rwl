using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface BlockFactory {
        StartBlock createStartBlock();
        MoveBlock createMoveBlock();
        WaitBlock createWaitBlock();
        LoopBlock createLoopBlock();
        SwitchBlock<T> createSwitchBlock<T>();
    }
}
