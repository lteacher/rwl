using System;
using System.Net.Sockets;
using RobotInitial.Model;
using System.Collections.Generic;
using System.Threading;
using RobotInitial.LynxProtocol;
using System.IO;

namespace RobotInitial.Lynx_Server {

    sealed class Virtual_Machine {

        #region Singleton Pattern
        private static volatile Virtual_Machine instance;
        private static object singletonLock = new Object();

        public static Virtual_Machine Instance {

            get {
                if (instance == null) {
                    lock (singletonLock) {
                        if (instance == null) {
                            instance = new Virtual_Machine();
                        }
                    }
                }

                return instance;
            }

        }

        private Virtual_Machine() { }

        #endregion

        //Volatile varibles must be read from memory and updated in a single command/cycle
        volatile Boolean runningProgram = false;
        volatile Shutdown terminate;

        int runningProgramID;
        private StartBlock Start;
        private Protocol Protocol;
        public EndState state;
		private ModelExecutor executor;

        object initialiseLock = new Object();
        object runningLock = new Object();

		public Boolean Initialise() {
			lock (initialiseLock) {
				if (runningProgram) {
					return false;
				}
				else {
					runningProgram = true;
					Start = null;
					Protocol = null;
					terminate = Shutdown.None;
					state = EndState.None;
					runningProgramID = Thread.CurrentThread.ManagedThreadId;
				}
			}

			return true;
		}

		public bool isInitial() {
			return runningProgram ? false : true;
		}

        public void pause() {
            executor.Pause();
        }

        public void resume() {
            executor.Resume();
        }

        public void LoadProgram(StartBlock start, Protocol protocol) {
            lock (runningLock) {
                this.Start = start;
                this.Protocol = protocol;
            }
        }

        //Hardware & Software reset of program
        public void TerminateProgram(Shutdown terminate) {
            //Calling thread must currently have ownership of the VM
            if (Thread.CurrentThread.ManagedThreadId == runningProgramID) {
                this.terminate = terminate;
				state = EndState.TerminatedByClient;
                executor.StopExecution();
            } else {
                throw new VirtualMachineOwnershipException();
            }
        }

        public void Reset() {
            //Calling thread must currently have ownership of the VM
            if (Thread.CurrentThread.ManagedThreadId == runningProgramID) {
                runningProgram = false;
				// On reset lets reset the state
				state = EndState.None;
            } else {
                throw new VirtualMachineOwnershipException();
            }
        }

        public void RunProgram() {
            lock (runningLock) {
                try {
                    LynxMessagePort.Instance.ClaimComPort();
                } catch (UnauthorizedAccessException e) {
                    Console.WriteLine("COM1 is in use by another process. The program will not be executed");
                    return;
                } catch (ComPortAlreadyClaimedException e) {
                    Console.WriteLine("The COM port was claimed without being released, due to an error occured on the server. The program will not be executed.");
                    return;
                } catch (IOException e) {
                    Console.WriteLine("An error has occured with the COM port, the program will not be executed");
                    return;
                }

                if (LynxMessagePort.Instance.HasComPort()) {

                    Console.Write("Running program \n");
                    executor = new ModelExecutor(Start, Protocol);

                    while (!executor.IsDone()) {
                        //Check hardware/software shutdowns
                        if (terminate != Shutdown.None) {
                            //executor.StopExecution(); now called on the request thread
                            state = EndState.TerminatedByClient;
                            break;
                        }

                        //Run through next program instruction
                        executor.ExecuteOneBlock();
                    }

                    if (state == EndState.None) state = EndState.Completed;
                    LynxMessagePort.Instance.ReleaseComPort();
                }
            }
        }
    }
}