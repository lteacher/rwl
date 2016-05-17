using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    interface ModelFactory {
        StartBlock CreateStartBlock();
        MoveBlock CreateMoveBlock();
        WaitBlock CreateWaitBlock();
        LoopBlock CreateLoopBlock();
        SwitchBlock<T> CreateSwitchBlock<T>();
        TrueConditional CreateTrueConditional();
        FalseConditional CreateFalseConditional();
        TimeConditional CreateTimeConditional();
        IRSensorConditional CreateIRSensorConditional();
        CountConditional CreateCountConditional();
        RBGConditional CreateRBGConditional();
        RNGConditional CreateRNGConditional();
    }
}
