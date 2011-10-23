using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.IO;
using System.IO.Ports;

namespace RobotInitial.LynxProtocol {
    //written when we had no idea how the lynx protocol actually works

    //class LegoProtocol : Protocol {
    //    private const int ENCODERCOUNTSPERREVOLUTION = 500;

    //    public LegoProtocol() {
    //    }

    //    ~LegoProtocol() {
    //    }

    //    private int duration2Encoder(MoveDurationUnit unit, float val) {
    //        switch (unit) {
    //            case MoveDurationUnit.ENCODERCOUNT:
    //                return (int)val;
    //            case MoveDurationUnit.DEGREES:
    //                return (int)((val / 360.0f) * ENCODERCOUNTSPERREVOLUTION);
    //            case MoveDurationUnit.UNLIMITED:
    //            default:
    //            case MoveDurationUnit.MILLISECONDS: //FIXME: may have to  manually time it....
    //                return 0;
    //        }
    //    }

    //    public void move(MoveParameters parameters) {
    //        byte[] args;

    //        if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
    //            args = new byte[2];
    //        } else {
    //            args = new byte[4];
    //            int dur = duration2Encoder(parameters.DurationUnit, parameters.Duration);
    //            args[2] = (byte)((dur & 0xFF00) >> 8);  //higher byte
    //            args[3] = (byte)(dur & 0x00FF);         //lower byte
    //        }

    //        args[0] = (byte)LegoCommandExt.getLeftCommand(parameters);
    //        args[1] = (byte)parameters.LeftPower;
    //        LynxMessage leftMsg = new LynxMessage(LynxCommandSet.LEGO, args);

    //        args[0] = (byte)LegoCommandExt.getRightCommand(parameters);
    //        args[1] = (byte)parameters.RightPower;
    //        LynxMessage rightMsg = new LynxMessage(LynxCommandSet.LEGO, args);

    //        sendMessage(leftMsg);
    //        sendMessage(rightMsg);
    //        getResponse();
    //        getResponse();//block until both complete
    //    }

    //    public IRData requestIR() {
    //        sendMessage(new LynxMessage(LynxCommandSet.LEGO, (byte)LegoCommand.REQIR));
    //        LynxMessage resp = getResponse();

    //        int[] distances = new int[resp.Length];
    //        for (int i = 0; i < distances.Length; ++i) {
    //            distances[i] = resp.getArg(0);
    //        }
    //        return new LynxIRData(distances);
    //    }

    //    public IMUData requestIMU() {
    //        sendMessage(new LynxMessage(LynxCommandSet.LEGO, (byte)LegoCommand.REQIMU));
    //        LynxMessage resp = getResponse();

    //        return new LynxIMUData(
    //            new Vector3(resp.getArg(0), resp.getArg(1), resp.getArg(2)),
    //            new Vector3(resp.getArg(3), resp.getArg(4), resp.getArg(5)),
    //            new Vector3(resp.getArg(6), resp.getArg(7), resp.getArg(8)));
    //    }

    //    public int requestStatus() {
    //        sendMessage(new LynxMessage(LynxCommandSet.LEGO, (byte)LegoCommand.REQSTATUS));
    //        LynxMessage resp = getResponse();
    //        return (resp.getArg(0) << 8) | resp.getArg(1);
    //    }
    //}
}
