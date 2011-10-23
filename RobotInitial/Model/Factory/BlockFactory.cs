using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface BlockFactory {
        StartBlock CreateStartBlock();
        MoveBlock CreateMoveBlock();
        WaitBlock CreateWaitBlock();
        LoopBlock CreateLoopBlock();
        SwitchBlock<T> CreateSwitchBlock<T>();
    }
}
