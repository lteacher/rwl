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
        private volatile Boolean pauseFlag = false;
		private ModelExecutor executor;

        object initialiseLock = new Object();
        object runningLock = new Object();

		//public Boolean Initialise() {
		//    lock (initialiseLock) {
		//        if (runningProgram) {
		//            return false;
		//        } else {
		//            runningProgram = true;
		//            Start = null;
		//            Protocol = null;
		//            terminate = Shutdown.None;
		//            state = EndState.None;
		//            runningProgramID = Thread.CurrentThread.ManagedThreadId;
		//        }
		//    }

		//    return true;
		//}

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
            pauseFlag = true;
        }

        public void resume() {
            pauseFlag = false;
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
                            executor.StopExecution();
                            state = EndState.TerminatedByClient;
                            break;
                        }

                        if (pauseFlag) {
                            executor.Pause();
                            while (pauseFlag) { }   //busy wait
                            executor.Resume();
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










//    public static Boolean runningProgram = false;
//    private static Boolean stopProgram = false;
//    private static Object thisLock = new Object();

//    private TcpClient client;

//    public static void Initialise(object input){

//        //Only one thread may attempt to initalise the VM at a time
//        lock (thisLock) {

//            //If the VM is currently running a program, stop.
//            if (runningProgram) {
//                return;
//            } else {
//                runningProgram = true;
//            }

//            //Cast our input object to the TcpClient and start the VM.
//            RWL_Virtual_Machine VM = new RWL_Virtual_Machine((TcpClient)input);
//            VM.Start();

//        }
//    }

//    public static void forceStop(){
//        stopProgram = true;
//    }

//    private RWL_Virtual_Machine(TcpClient client) {
//        this.client = client;
//    }

//    private void Start() {
//        while (!stopProgram) {

//        }
//    }
//}

//Grab the start block from the workspace
//LinkedList<Block> ProgramList = new LinkedList<Block>();
//LynxProtocol protocol = new LynxProtocol();
//Block currentBlock;

//program._startBlock.perform(protocol, ref ProgramList);

//while (ProgramList.Count > 0) {
//    if (terminate) {
//        state = EndState.TerminatedByClient;
//        return;
//    }

//    //Pop the first item on the stack/list and run it
//    currentBlock = ProgramList.First.Value;
//    ProgramList.RemoveFirst();
//    currentBlock.perform(protocol, ref ProgramList);
//}
