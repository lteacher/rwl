using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Model {
    //was be used as a memento for undo/redo
    //not used anymore
    class _ParameterList {
        private Dictionary<String, Object> parameters = new Dictionary<String, Object>();

        public Object this[String key] {
            get { return parameters.ContainsKey(key) ? parameters[key] : null; }

            set {
                if (parameters.ContainsKey(key)) {
                    parameters[key] = value;
                } else {
                    parameters.Add(key, value);
                }
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder("{ ");

            foreach (Object o in parameters) {
                builder.Append(o.ToString()).Append(", ");
            }

            return builder.Append("}").ToString();
        }
    }
}
