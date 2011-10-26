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

        private const int CONTINUOUSREQUESTDELAY = 400;
        private readonly MessageFactory factory = new LynxMessageFactory();
        private volatile Thread continuousMovementThread = null;
        private LynxMessage currentContCmdL = null;
        private LynxMessage currentContCmdR = null;
        private Object continuousLock = new Object();

        #endregion

        #region Implemented Protocol Methods

        public void Move(MoveParameters parameters) {
            LynxMessage l = factory.CreateMoveMsg(parameters, Side.LEFT);
            LynxMessage r = factory.CreateMoveMsg(parameters, Side.RIGHT);
            
            if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
                StartContinuousMovement(l, r);
            } else {
                StopContinuousMovement();
                SendMovement(l, r);
                WaitForCompletion();
            }
        }

        public IRData RequestIR() {
            LynxMessage r = LynxMessagePort.Instance.Send(factory.CreateIRReq(), true);

            int[] distances = new int[r.Length - 1];
            for (int i = 0; i < distances.Length; ++i) {
                distances[i] = r.GetArg(i + 1);
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
            StopContinuousMovement();
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
            //This lock prevent left and right messages from being inturrupted by a status request generated from the continuous movement thread.
            //The port locks on itself while a messsage is being sent.
            //C# locks are re-entrant locks.
            lock (LynxMessagePort.Instance) {
                LynxMessagePort.Instance.Send(left, false);
                LynxMessagePort.Instance.Send(right, false);
            }
        }

        private LynxStatus RequestStatus(Side side) {
            LynxMessage resp = LynxMessagePort.Instance.Send(factory.CreateStatusReq(side), true);
            return (LynxStatus)((resp.GetArg(0) << 8) | resp.GetArg(1));
        }

        #endregion

        #region Continuous Movement Methods

        private void StartContinuousMovement(LynxMessage left, LynxMessage right) {
            bool leftDif = !left.Equals(currentContCmdL);
            bool rightDif = !right.Equals(currentContCmdR);

            if (leftDif && rightDif) {
                //send both with this method so they don't get inturrupted
                SendMovement(left, right);
                currentContCmdL = left;
                currentContCmdR = right;
            } else {
                if (leftDif) {
                    LynxMessagePort.Instance.Send(left, false);
                    currentContCmdL = left;
                } else if (rightDif) {
                    LynxMessagePort.Instance.Send(right, false);
                    currentContCmdR = right;
                }
            }

            if (continuousMovementThread == null) {
                continuousMovementThread = new Thread(ContinuousMovementThread);
                continuousMovementThread.Start();
            }
        }

        private void StopContinuousMovement() {
            continuousMovementThread = null;
            currentContCmdL = null;
            currentContCmdR = null;
        }

        private void ContinuousMovementThread() {
            //This thread stops if the continuousMovementThread was set to null,
            //or in the unlikely event a new thread was started before this was able to stop 
            //(which is only possible if it was set it to null and then a new thread was started before the old one could finish)
            //The lock prevents request spam if multiple threads were created, if that unlikely event were to occur.
            lock (continuousLock) {
                while (Thread.CurrentThread.Equals(continuousMovementThread)) {
                    RequestStatus(Side.LEFT);
                    RequestStatus(Side.RIGHT);
                    Thread.Sleep(CONTINUOUSREQUESTDELAY);
                }
            }
        }

        #endregion

    }
}
