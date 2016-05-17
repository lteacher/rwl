using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;

namespace RobotInitial.LynxProtocol {
    interface MessageFactory {
        LynxMessage CreateMoveMsg(MoveParameters parameters, Side side);
        LynxMessage CreateBrakeMsg(Side side);
        LynxMessage CreateStatusReq(Side side);
        LynxMessage CreateIMUReq();
        LynxMessage CreateIRReq();
        //LynxMessage CreateKeepAlive();    using status instead
    }
}
