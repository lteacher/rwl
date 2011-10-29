using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Diagnostics;

namespace RobotInitial.LynxProtocol {
    class LynxProtocol : Protocol {

        #region Fields

        private readonly MessageFactory factory = new LynxMessageFactory();
        private LynxMessage currentContCmdL = null;
        private LynxMessage currentContCmdR = null;

        #endregion

        #region Implemented Protocol Methods

        public void Move(MoveParameters parameters) {
            LynxMessage l = factory.CreateMoveMsg(parameters, Side.LEFT);
            LynxMessage r = factory.CreateMoveMsg(parameters, Side.RIGHT);

            if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
                StartContinuousMovement(l, r);
            } else {
                ClearContinuousMovement(); 
                SendMovement(l, r);
                WaitForCompletion();
            }
        }

        public IRData RequestIR() {
            LynxMessage r = LynxMessagePort.Instance.Send(factory.CreateIRReq(), true);

            int[] distances = new int[r.Length - 1];
            for (int i = 0; i < distances.Length; ++i) {
                distances[i] = r.GetArg(i + 1); //ignore return type, which is in the first argument
            }

            return new LynxIRData(distances);
        }

        public IMUData RequestIMU() {
            LynxMessage r = LynxMessagePort.Instance.Send(factory.CreateIMUReq(), true);
            return new LynxIMUData(
                new Vector3(r.GetArg(1), r.GetArg(2), r.GetArg(3)),
                new Vector3(r.GetArg(4), r.GetArg(5), r.GetArg(6)),
                new Vector3(r.GetArg(7), r.GetArg(8), r.GetArg(9)));
        }

        //need to test how the robot reacts to the reset commands before I can write this
        public void OnExecutionStart() {
            Console.WriteLine("PLEASE WRITE THE INITILISATION CODE");
            //send reset
            //wait for ready
        }

        public void OnExecutionFinish() {
            Console.WriteLine("program ended");
            SendMovement(factory.CreateBrakeMsg(Side.LEFT), factory.CreateBrakeMsg(Side.RIGHT));
        }

        //the lynx status doesn't contain any useful info for the program
        public int RequestStatus() {
            return 0;
        }

        #endregion

        #region Helper Methods

        private void WaitForCompletion() {
            //continuously poll until both motors return the complete status
            bool leftIncomplete = true;
            bool rightIncomplete = true;
            while (leftIncomplete || rightIncomplete) {
                if (leftIncomplete) {
                    leftIncomplete = RequestStatus(Side.LEFT).HasFlag(LynxStatus.COMMAND_INCOMPLETE);
                }
                if (rightIncomplete) {
                    rightIncomplete = RequestStatus(Side.RIGHT).HasFlag(LynxStatus.COMMAND_INCOMPLETE);
                }
            }
        }

        private void SendMovement(LynxMessage left, LynxMessage right) {
            LynxMessagePort.Instance.Send(left, false);
            LynxMessagePort.Instance.Send(right, false);
        }

        private LynxStatus RequestStatus(Side side) {
            LynxMessage resp = LynxMessagePort.Instance.Send(factory.CreateStatusReq(side), true);
            return (LynxStatus)((resp.GetArg(0) << 8) | resp.GetArg(1));
        }

        #endregion

        #region Continuous Movement Methods

        private void StartContinuousMovement(LynxMessage left, LynxMessage right) {
            //only send if it is different to the current continuous command
            if (!left.Equals(currentContCmdL)) {
                LynxMessagePort.Instance.Send(left, false);
                currentContCmdL = left;
            }
            if (!right.Equals(currentContCmdR)) {
                LynxMessagePort.Instance.Send(right, false);
                currentContCmdR = right;
            }
        }

        private void ClearContinuousMovement() {
            currentContCmdL = null;
            currentContCmdL = null;
        }

        //private void StopContinuousMovement() {
        //    if (currentContCmdL != null) {
        //        LynxMessagePort.Instance.Send(factory.CreateBrakeMsg(Side.LEFT), false);
        //        currentContCmdL = null;
        //    }
        //    if (currentContCmdR != null) {
        //        LynxMessagePort.Instance.Send(factory.CreateBrakeMsg(Side.RIGHT), false);
        //        currentContCmdR = null;
        //    }
        //}

        #endregion

    }
}
