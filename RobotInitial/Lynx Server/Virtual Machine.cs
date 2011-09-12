using System;
using System.Net.Sockets;
using RobotInitial.Model;
using System.Collections.Generic;

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
        #endregion

        //Volatile varibles must be read from memory and updated in a single command/cycle
        volatile Boolean runningProgram = false;
        volatile Boolean terminate = false;

        int runningProgramID;
        Workspace program;       
        public EndState state;

        object initialiseLock = new Object();
        object runningLock = new Object();

        public Boolean Initialise(TcpClient client, int threadID) {
            lock (initialiseLock) {
                if (runningProgram) {
                    return false;
                } else {
                    runningProgram = true;
                    program = null;
                    this.runningProgramID = threadID;
                }
            }

            return true;     
        }

        public void LoadProgram(Workspace program) {
            lock (runningLock) {
                this.program = program;
            }            
        }

        public void TerminateProgram() {
            terminate = true;
        }

        public void Reset() {
            runningProgram = false;
        }

        public void RunProgram() {
            lock (runningLock) {
                //Grab the start block from the workspace
                LinkedList<Block> ProgramList = new LinkedList<Block>();
                LynxProtocol protocol = new LynxProtocol();
                Block currentBlock;

                program._startBlock.perform(protocol, ref ProgramList);

                while (ProgramList.Count > 0) {
                    if (terminate) {
                        state = EndState.TerminatedByClient;
                        return;
                    }

                    //Pop the first item on the stack/list and run it
                    currentBlock = ProgramList.First.Value;
                    ProgramList.RemoveFirst();
                    currentBlock.perform(protocol, ref ProgramList);
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
