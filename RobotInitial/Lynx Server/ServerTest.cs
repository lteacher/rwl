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

            //LoopBlock loop = new LoopBlock();
            //CountConditional count = new CountConditional();
            //count.Limit = 10;
            //loop.Condition = count;
            //MoveBlock truepath = new MoveBlock();
            //loop.LoopPath = truepath;
            //start.Next = loop;

            DefaultBlockFactory fact = DefaultBlockFactory.Instance;

            StartBlock start = fact.createStartBlock();
            LoopBlock loop = fact.createLoopBlock();
            MoveBlock move = fact.createMoveBlock();
            //move.DurationUnit = MoveDurationUnit.DEGREES;
            //move.Duration = 360.0f;
            loop.LoopPath = move;
            start.Next = loop;

            Console.Write("Contacting Server \n");
            Network.connectToLynx(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7331), start);

            //Keep Console Open
            while (true) {
            }
        }
    }
}
