using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    class SwitchBlock<T> : AbstractCompositeBlock {

        #region Fields

        private Dictionary<T, Block> mappedPaths = new Dictionary<T, Block>();
        public T DefaultPath { get; set; }
        public Conditional<T> Condition { get; set; }

        public override Block NextToPerform {
            get { return Next; }
        }

        private Block pathToPerform = null;
        public override Block PathToPerform {
            get { return pathToPerform; }
        }

        public override List<Block> Paths {
            get { return mappedPaths.Values.ToList(); }
        }

        #endregion

        #region Constructors



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

        #endregion
    }
}
