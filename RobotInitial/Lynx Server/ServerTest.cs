using System.Diagnostics;
using System;
using RobotInitial.Model;
using LynxTest2.Communications;
using System.Net;
using System.Threading;

namespace RobotInitial.Lynx_Server {
    class ServerTest {
        static void Main() {
            Thread ServerThread;
            Lynx_Server server = new Lynx_Server();

            ServerThread = new Thread(server.start);
            ServerThread.Start();
            Console.Write("Server started \n");


            //StartBlock start = new StartBlock();
            //StartBlock start = new StartBlock();

            //LoopBlock loop = new LoopBlock();
            //CountConditional count = new CountConditional();
            //count.Limit = 10;
            //loop.Condition = count;
            //MoveBlock truepath = new MoveBlock();
            //loop.LoopPath = truepath;
            //start.Next = loop;

            //DefaultModelFactory fact = DefaultModelFactory.Instance;

            //StartBlock start = fact.CreateStartBlock();
            //LoopBlock loop = fact.CreateLoopBlock();
            //MoveBlock move = fact.CreateMoveBlock();
            //move.DurationUnit = MoveDurationUnit.ENCODERCOUNT;
            //move.Duration = 5000;
            //move.DurationUnit = MoveDurationUnit.DEGREES;
            //move.Duration = 180.0f;
            //move.DurationUnit = MoveDurationUnit.UNLIMITED;
            //move.Duration = 3000;
            //move.DurationUnit = MoveDurationUnit.MILLISECONDS;
            //move.LeftDirection = MoveDirection.STOP;
            //move.RightDirection = MoveDirection.STOP;
            //loop.LoopPath = move;
            //start.Next = move;

            //WaitBlock wait = fact.CreateWaitBlock();
            //wait.WaitUntil = new FalseConditional();
            //move.Next = wait;

            //Console.Write("Contacting Server \n");
            //Network.connectToLynx(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7331), start);

            //Keep Console Open
            //while (true) {
            //}
        }
    }
}
