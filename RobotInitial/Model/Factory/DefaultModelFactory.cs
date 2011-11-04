using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Singleton class with lazy instantiation, using static initilisation 
//as described in the msdn here: http://msdn.microsoft.com/en-us/library/ms998558.aspx
namespace RobotInitial.Model {
    //creates blocks with default parameters
    sealed class DefaultModelFactory :ModelFactory {
        //private constructor
        private DefaultModelFactory() { }

        private static readonly DefaultModelFactory instance = new DefaultModelFactory();
        public static DefaultModelFactory Instance {
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
            block.LeftDuration = 3200;
            block.RightDuration = 3200;
            block.DurationUnit = MoveDurationUnit.ENCODERCOUNT;
            block.BrakeAfterMove = true;
            return block;
        }

        public WaitBlock CreateWaitBlock() {
            WaitBlock block = new WaitBlock();
            block.WaitUntil = this.CreateTimeConditional();
            return block;
        }

        public LoopBlock CreateLoopBlock() {
            LoopBlock block = new LoopBlock();
            block.Condition = this.CreateCountConditional();
            return block;
        }

        public SwitchBlock<T> CreateSwitchBlock<T>() {
            SwitchBlock<T> block = new SwitchBlock<T>();
            Type type = typeof(T);

            if (type.Equals(typeof(bool))) {
                (block as SwitchBlock<bool>).Condition = this.CreateIRSensorConditional();
            } else if (type.Equals(typeof(int))) {
                (block as SwitchBlock<int>).Condition = this.CreateRNGConditional();     //change to a more useful conditional, if one ever gets created
            }

            return block;
        }


        public TrueConditional CreateTrueConditional() {
            return new TrueConditional();
        }

        public FalseConditional CreateFalseConditional() {
            return new FalseConditional();
        }

        public TimeConditional CreateTimeConditional() {
            TimeConditional cond = new TimeConditional();
            cond.Duration = 1000;
            return cond;
        }

        public IRSensorConditional CreateIRSensorConditional() {
            IRSensorConditional cond = new IRSensorConditional();
            cond.SetDistance(LynxIRPort.FRONT, 50);
            cond.SetPortState(LynxIRPort.FRONT, true);
            cond.EqualityOperator = Operator.LESS;
            cond.LogicalOperator = LogicalOperator.OR;
            return cond;
        }

        public CountConditional CreateCountConditional() {
            CountConditional cond = new CountConditional();
            cond.Limit = 4;
            return cond;
        }

        public RBGConditional CreateRBGConditional() {
            RBGConditional cond = new RBGConditional();
            return cond;
        }

        public RNGConditional CreateRNGConditional() {
            RNGConditional cond = new RNGConditional();
            cond.Max = 1;
            cond.Max = 3;
            return cond;
        }
    }
}