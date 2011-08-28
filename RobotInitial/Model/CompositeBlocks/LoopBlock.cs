using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //do until loop
    class LoopBlock : AbstractCompositeBlock {
       
        #region Fields

        private bool initilised = false;
        public Conditional<bool> Condition { get; set; }
        public Block LoopPath { get; set; }

        private Block nextToPerform = null;
        public override Block NextToPerform {
            get { return nextToPerform; }
        }

        
        public override Block PathToPerform {
            get { return LoopPath; }
        }

        #endregion
        
        #region Constructors

        public LoopBlock() {
            Condition = new CountConditional();
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

        #endregion 

    }
}
