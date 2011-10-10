using System;
using System.Net.Sockets;
using RobotInitial.Model;
using System.Collections.Generic;
using System.Threading;

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
        volatile Shutdown terminate;

        int runningProgramID;
        private StartBlock Start;
        private Protocol Protocol;     
        public EndState state;

        object initialiseLock = new Object();
        object runningLock = new Object();

        public Boolean Initialise() {
            lock (initialiseLock) {
                if (runningProgram) {
                    return false;
                } else {
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

        public void LoadProgram(StartBlock start, Protocol protocol) {
            lock (runningLock) {
                this.Start = start;
                this.Protocol = protocol;
            }            
        }

        //Hardware & Software reset of program
        public void TerminateProgram(Shutdown terminate) {
            //Calling thread must currently have ownership of the VM
            if(Thread.CurrentThread.ManagedThreadId == runningProgramID){
                this.terminate = terminate;
            }else{
                throw new VirtualMachineOwnershipException();
            }
        }

        public void Reset() {
            //Calling thread must currently have ownership of the VM
            if(Thread.CurrentThread.ManagedThreadId == runningProgramID){
                runningProgram = false;
            }else{
                throw new VirtualMachineOwnershipException();
            } 
        }

        public void RunProgram() {
            lock (runningLock) {
                Console.Write("Running program \n");
                ModelExecutor executor = new ModelExecutor(Start, Protocol);

                while (!executor.isDone()) {
                    //Console.Write("Program Line Run \n");
                    //Check hardware/software shutdowns
                    if (terminate != Shutdown.None){

                        if(terminate == Shutdown.Software){
                            state = EndState.TerminatedByClient;
                        }else{
                            state = EndState.TerminatedByHardware;
                        }

                        break;
                    }

                    //Run through next program instruction
                    executor.executeOneBlock();
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
