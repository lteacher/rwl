using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Singleton class with lazy instantiation, using static initilisation 
//as described in the msdn here: http://msdn.microsoft.com/en-us/library/ms998558.aspx
namespace RobotInitial.Model {
    //creates blocks with default parameters
    sealed class DefaultBlockFactory : BlockFactory {
        //private constructor
        private DefaultBlockFactory() { }

        private static readonly DefaultBlockFactory instance = new DefaultBlockFactory();
        public static DefaultBlockFactory Instance {
            get { return instance; }
        }

        public StartBlock CreateStartBlock() {
            return new StartBlock();
        }

        public MoveBlock CreateMoveBlock() {
            MoveBlock block = new MoveBlock();
            block.LeftDirection = MoveDirection.FORWARD;
            block.RightDirection = MoveDirection.FORWARD;
            block.LeftPower = 50;
            block.RightPower = 50;
            block.LeftDuration = 500;
            block.RightDuration = 500;
            block.DurationUnit = MoveDurationUnit.ENCODERCOUNT;
            block.BrakeAfterMove = true;
            return block;
        }

        public WaitBlock CreateWaitBlock() {
            WaitBlock block = new WaitBlock();
            TimeConditional cond = new TimeConditional();
            cond.Duration = 1000;
            block.WaitUntil = cond;
            return block;
        }

        public LoopBlock CreateLoopBlock() {
            LoopBlock block = new LoopBlock();
            CountConditional cond = new CountConditional();
            cond.Limit = 5;
            block.Condition = cond;
            return block;
        }

        public SwitchBlock<T> CreateSwitchBlock<T>() {
            SwitchBlock<T> block = new SwitchBlock<T>();
            Type type = typeof(T);

            if (type.Equals(typeof(bool))) {
                IRSensorConditional cond = new IRSensorConditional();
                cond.Distance = 50;
                cond.IRSensorNumber = 0;
                cond.EqualityOperator = IRSensorConditional.Operator.LESS;
                (block as SwitchBlock<bool>).Condition = cond;
            } else if (type.Equals(typeof(int))) {
                (block as SwitchBlock<int>).Condition = new RNGConditional();     //change to a more useful conditional, if one ever gets created
            }

            return block;
        }
    }
}