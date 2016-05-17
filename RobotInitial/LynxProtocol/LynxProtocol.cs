using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobotInitial.Model;
using System.Diagnostics;
using System.Threading;

namespace RobotInitial.LynxProtocol {
    class LynxProtocol : Protocol {

        #region Fields

        private readonly MessageFactory factory = new LynxMessageFactory();
        private LynxMessage currentContCmdL = null;
        private LynxMessage currentContCmdR = null;
        private LynxMessage pausedContCmdL = null;
        private LynxMessage pausedContCmdR = null;
        private bool brakeFlagL = false;
        private bool brakeFlagR = false;
        private volatile bool pauseFlag = false;
        private volatile bool stopFlag = false;
        private readonly Object movementLock = new Object();
        private readonly Object flagReadLock = new Object();

        #endregion

        #region Constructor

        public LynxProtocol() {
        }

        #endregion

        #region Implemented Protocol Methods

        //this junk is only needed because the brake commands are screwed and we are using a hack instead.
        //only send brake commands when the last command was not a brake.
        private static void NullifyMessageIfFlaggedHack(MoveDirection direction, ref bool brakeFlag, ref LynxMessage message) {
            if (direction == MoveDirection.STOP) {
                if (brakeFlag) {
                    message = null;
                }
                brakeFlag = true;
            } else {
                brakeFlag = false;
            }
        }

        public void Move(MoveParameters parameters) {
            lock (movementLock) {
                if (ShouldStop()) {
                    return;
                }

                LynxMessage l = factory.CreateMoveMsg(parameters, Side.LEFT);
                LynxMessage r = factory.CreateMoveMsg(parameters, Side.RIGHT);

                //nullify any stop messages if it already stopped.
                //obliterate this junk when the brake commands actually work, instead of using our move a very small distance hack
                NullifyMessageIfFlaggedHack(parameters.LeftDirection, ref brakeFlagL, ref l);
                NullifyMessageIfFlaggedHack(parameters.RightDirection, ref brakeFlagR, ref r);

                if (parameters.DurationUnit == MoveDurationUnit.UNLIMITED) {
                    StartContinuousMovement(l, r);
                } else {
                    SendEncoderMovement(l, r);
                    WaitForCompletion();
                }
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


        public int RequestStatus() {
            //the lynx status doesn't contain any useful info for the program
            return 0;
        }

        //need to test how the robot reacts to the reset commands before I can write this
        public void OnExecutionStart() {
            Console.WriteLine("PLEASE WRITE THE INITILISATION CODE");
            //send reset
            //wait for ready
        }

        public void OnExecutionFinish() {
            //flag stop before lock attempt to pre-empt any movement commands
            bool executeStop;
            lock (flagReadLock) {
                executeStop = !stopFlag;
                stopFlag = true;
            }

            if (executeStop) {
                lock (movementLock) {
                    SendBrake();
                }
            }
        }

        public void Pause() {
            //flag pause before lock to pre-empt any movement commands
            bool executePause;
            lock (flagReadLock) {
                executePause = !pauseFlag;
                pauseFlag = true;
            }

            if (executePause) {
                lock (movementLock) {
                    PauseContinuousMovement();
                }
            }
        }

        public void Resume() {
            //have to do this because there is no Interlocked.Exchange for bools
            //and I cant make my own exchange with a ref because then the flag will not be considered volatile
            bool executeResume;
            lock (flagReadLock) {
                executeResume = pauseFlag;
                pauseFlag = false;
            }

            if (executeResume) {
                lock (movementLock) {
                    ResumeContinuousMovement();
                }
            }
        }

        #endregion

        #region Helper Methods

        private bool ShouldStop() {
            return pauseFlag || stopFlag;
        }

        private void WaitForCompletion() {
            //continuously poll until both motors return the complete status
            bool leftIncomplete = true;
            bool rightIncomplete = true;
            while ((leftIncomplete || rightIncomplete) && !ShouldStop()) {
                if (leftIncomplete) {
                    leftIncomplete = RequestStatus(Side.LEFT).HasFlag(LynxStatus.COMMAND_INCOMPLETE);
                }
                if (rightIncomplete) {
                    rightIncomplete = RequestStatus(Side.RIGHT).HasFlag(LynxStatus.COMMAND_INCOMPLETE);
                }
            }
        }

        private void SendEncoderMovement(LynxMessage left, LynxMessage right) {
            if (left != null) {
                LynxMessagePort.Instance.Send(left, false);
            }
            if (right != null) {
                LynxMessagePort.Instance.Send(right, false);
            }
            ClearContinuousMovement();
        }

        private void SendBrake() {
            //only send the brake when it is actually moving
            //if (IsContinuouslyMoving()) {
                LynxMessagePort.Instance.Send(factory.CreateBrakeMsg(Side.LEFT), false);
                LynxMessagePort.Instance.Send(factory.CreateBrakeMsg(Side.RIGHT), false);
                ClearContinuousMovement();
            //}
        }

        private LynxStatus RequestStatus(Side side) {
            LynxMessage resp = LynxMessagePort.Instance.Send(factory.CreateStatusReq(side), true);
            return (LynxStatus)((resp.GetArg(0) << 8) | resp.GetArg(1));
        }

        #endregion

        #region Continuous Movement Methods

        private void StartContinuousMovement(LynxMessage left, LynxMessage right) {
            //only send if it is different to the current continuous command
            if (left != null && !left.Equals(currentContCmdL)) {
                currentContCmdL = left;
                LynxMessagePort.Instance.Send(left, false);
            }
            if (right != null && !right.Equals(currentContCmdR)) {
                currentContCmdR = right;
                LynxMessagePort.Instance.Send(right, false);
            }
        }

        private void PauseContinuousMovement() {
            pausedContCmdL = currentContCmdL;
            pausedContCmdR = currentContCmdR;
            this.SendBrake(); //clears currentContCmdL/R
        }

        private void ResumeContinuousMovement() {
            StartContinuousMovement(pausedContCmdL, pausedContCmdR);
            pausedContCmdL = null;
            pausedContCmdR = null;
        }

        private void ClearContinuousMovement() {
            currentContCmdL = null;
            currentContCmdR = null;
        }

        //private bool IsLeftContinuouslyMoving() {
        //    return currentContCmdL != null;
        //}

        //private bool IsRightContinuouslyMoving() {
        //    return currentContCmdR != null;
        //}

        private bool IsContinuouslyMoving() {
            return currentContCmdR != null || currentContCmdL != null;
        }

        #endregion
    }
}
