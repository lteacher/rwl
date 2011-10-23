using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Threading;
using System.Diagnostics;

namespace RobotInitial.LynxProtocol {
    class LynxProtocol : Protocol {

        #region Fields

        private const int WAITCOMPLETIONINTERVAL = 25;
        private readonly LynxMessageFactory factory = new RawMessageFactory();
        private Stopwatch aliveTimer = new Stopwatch();
        private const int LIFETIME = 1000;

        #endregion

        #region Implemented Protocol Methods

        public void Move(MoveParameters parameters) {
            LynxMessage l = factory.CreateMoveMsg(parameters, Side.LEFT);
            LynxMessage r = factory.CreateMoveMsg(parameters, Side.RIGHT);
            SendIfAlive(l, false);
            SendIfAlive(r, false);

            switch (parameters.DurationUnit) {
                case MoveDurationUnit.UNLIMITED:
                    break;
                case MoveDurationUnit.MILLISECONDS:
                    Thread.Sleep((int)parameters.Duration);
                    SendIfAlive(factory.CreateBrakeMsg(Side.LEFT), false);
                    SendIfAlive(factory.CreateBrakeMsg(Side.RIGHT), false);
                    break;
                case MoveDurationUnit.DEGREES:
                case MoveDurationUnit.ENCODERCOUNT:
                default:
                    WaitForCompletion();
                    break;
            }
        }

        public IRData RequestIR() {
            LynxMessage r = SendIfAlive(factory.CreateIRReq(), true);

            int[] distances = new int[r.Length];
            for (int i = 0; i < distances.Length; ++i) {
                distances[i] = r.GetArg(i);
            }

            return new LynxIRData(distances);
        }

        public IMUData RequestIMU() {
            LynxMessage r = SendIfAlive(factory.CreateIMUReq(), true);
            return new LynxIMUData(
                new Vector3(r.GetArg(0), r.GetArg(1), r.GetArg(2)),
                new Vector3(r.GetArg(3), r.GetArg(4), r.GetArg(5)),
                new Vector3(r.GetArg(6), r.GetArg(7), r.GetArg(8)));
        }

        //use Current Direction?
        public int RequestStatus() {
            throw new NotImplementedException();
        }

        #endregion

        #region KeepAlive Stuff

        public void KeepAlive() {
            lock (aliveTimer) {
                aliveTimer.Restart();
            }
            RequestStatus(Side.LEFT);
            RequestStatus(Side.RIGHT);
        }

        public bool IsAlive() {
            bool result;
            lock (aliveTimer) {
                result = aliveTimer.IsRunning && aliveTimer.ElapsedMilliseconds <= LIFETIME;
            }
            return result;
        }

        #endregion

        #region Helper Methods

        //all messages sent through this method
        //for easy keep alive management
        private LynxMessage SendIfAlive(LynxMessage m, bool isRequest) {
            if (this.IsAlive()) {
                return LynxMessagePort.Instance.Send(m, isRequest);
            } else {
                Console.WriteLine("Protocol was not kept alive");
                throw new LynxIsNotAliveException();
            }
        }

        private void WaitForCompletion() {
            bool lDone = false;
            bool rDone = false;
            while (!rDone || !lDone) {
                if (!lDone) {
                    lDone = (RequestStatus(Side.LEFT) & LynxStatus.COMMAND_INCOMPLETE) == 0;
                }
                if (!rDone) {
                    rDone = (RequestStatus(Side.RIGHT) & LynxStatus.COMMAND_INCOMPLETE) == 0;
                }
                Thread.Sleep(WAITCOMPLETIONINTERVAL);
            }
        }


        private LynxStatus RequestStatus(Side side) {
            LynxMessage resp = SendIfAlive(factory.CreateStatusReq(side), true);
            return (LynxStatus)((resp.GetArg(0) << 8) | resp.GetArg(1));
        }

        #endregion
        /*
        //hack to simulate keepalive
        public LynxProtocol() {
            this.KeepAlive();
            new Thread(this.DeleteThisHack).Start();
        }

        private void DeleteThisHack() {
            while (true) {
                //Console.WriteLine("before Alive? " + this.IsAlive());
                this.KeepAlive();
                //Console.WriteLine("after Alive? " + this.IsAlive());
                Thread.Sleep(50);
            }
        }*/
    }
}
