using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //do until loop
    [Serializable()]
    class LoopBlock : AbstractCompositeBlock {

        #region Execution Fields

        private bool initilised = false;

        #endregion

        #region Parameter Fields

        public Conditional<bool> Condition { get; set; }
        public Block LoopPath { get; set; }

        #endregion

        #region Constructors

        internal LoopBlock() {
        }

        protected LoopBlock(LoopBlock other)
            : base(other) {
            this.Condition = (other.Condition == null) ? null : other.Condition.Clone() as Conditional<bool>;
            this.LoopPath = (other.LoopPath == null) ? null : other.LoopPath.Clone() as Block;

            //Execution Fields not really needed, but will copy so it is a real copy
            this.initilised = other.initilised;
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

        public override void perform(Protocol protocol, ref LinkedList<Block> performAfter) {
            performAfter.AddFirst(LoopPath); //always executed atlteast once...

            if (!initilised) {
                initilised = true;
                Condition.initilize();
            }
            Condition.update();

            //using do until logic
            if (Condition.evaluate(protocol)) {
                initilised = false;
                performAfter.AddLast(Next); //terminate loop
            } else {
                performAfter.AddLast(this); //evaluate loop again (after LoopPath path has been executed)
            }
        }

        public override object Clone() {
            return new LoopBlock(this);
        }

        #endregion

    }
}
