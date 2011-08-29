using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //do until loop
    class LoopBlock : AbstractCompositeBlock {

        #region Execution Fields

        private bool initilised = false;

        private Block nextToPerform = null;
        public override Block NextToPerform {
            get { return nextToPerform; }
        }

        public override Block InnerPathToPerform {
            get { return LoopPath; }
        }

        #endregion

        #region Parameter Fields

        public Conditional<bool> Condition { get; set; }
        public Block LoopPath { get; set; }

        #endregion

        #region Constructors

        public LoopBlock() {
            Condition = new CountConditional();
        }

        protected LoopBlock(LoopBlock other)
            : base(other) {
            this.Condition = (other.Condition == null) ? null : other.Condition.Clone() as Conditional<bool>;
            this.LoopPath = (other.LoopPath == null) ? null : other.LoopPath.Clone() as Block;

            //Execution Fields not really needed, but will copy so it is a real copy
            this.initilised = other.initilised;
            this.nextToPerform = other.nextToPerform;
        }

        #endregion

        #region Methods

        public override List<Block> Paths {
            get {
                List<Block> list = new List<Block>();
                list.Add(LoopPath);
                return list;
            }
        }

        public override void perform(Protocol protocol) {
            if (!initilised) {
                initilised = true;
                Condition.initilize();
            }
            Condition.update();

            //using do until logic
            if (Condition.evaluate(protocol)) {
                initilised = false;
                nextToPerform = Next;   //exit loop
            } else {
                nextToPerform = this;   //evaluate loop again (after ChildToPerform path has been executed)
            }
        }

        public override object Clone() {
            return new LoopBlock(this);
        }

        #endregion

    }
}
