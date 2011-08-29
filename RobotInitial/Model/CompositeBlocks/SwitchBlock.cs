using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class SwitchBlock<T> : AbstractCompositeBlock {

        #region Execution Fields

        public override Block NextToPerform {
            get { return Next; }
        }

        private Block pathToPerform = null;
        public override Block InnerPathToPerform {
            get { return pathToPerform; }
        }

        #endregion

        #region Parameter Fields

        private Dictionary<T, Block> mappedPaths = new Dictionary<T, Block>();
        public Conditional<T> Condition { get; set; }
        public T DefaultPath { get; set; }

        public override List<Block> Paths {
            get { return mappedPaths.Values.ToList(); }
        }

        #endregion

        #region Constructors

        public SwitchBlock() {
        }

        protected SwitchBlock(SwitchBlock<T> other) : base(other) {
            this.pathToPerform = other.pathToPerform;   //not really needed, execution variable
            this.DefaultPath = other.DefaultPath;       //may only be a shallow copy, should be okay. Anything used as a path map should be immutable anyway.
            this.Condition = (other.Condition == null) ? null : other.Condition.Clone() as Conditional<T>;

            foreach (KeyValuePair<T, Block> kvp in other.mappedPaths) {
                this.mappedPaths[kvp.Key] = (kvp.Value == null) ? null : kvp.Value.Clone() as Block;
            }
        }

        #endregion

        #region Methods

        public void mapPath(T t, Block path) {
            mappedPaths[t] = path;
        }

        public override void perform(Protocol protocol) {
            Condition.initilize();
            Condition.update();
            T result = Condition.evaluate(protocol);

            if (mappedPaths.ContainsKey(result)) {
                pathToPerform = mappedPaths[result];
            } else {
                pathToPerform = mappedPaths.ContainsKey(DefaultPath) ? mappedPaths[DefaultPath] : mappedPaths.First().Value;
            }
        }

        public override object Clone() {
            return new SwitchBlock<T>(this);
        }

        #endregion
    }
}
