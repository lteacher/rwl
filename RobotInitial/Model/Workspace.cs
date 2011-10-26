using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Properties;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RobotInitial.Model
{
    [Serializable()]
    public class Workspace : ICloneable
    {

        #region Static Accessors

        public static Workspace CreateNewWorkspace()
        {
            Workspace workspace = new Workspace();
            workspace.FileName = Resources.untitledFileName + Resources.fileExtension;
            workspace.IsUntitled = true;
            return workspace;
        }

        public static Workspace LoadWorkspace(string filename)
        {
            Workspace workspace = new Workspace();
            workspace.FileName = filename;
            workspace.IsUntitled = false;
            return workspace;
        }

        public static void SaveWorkspace(string filename, Workspace workspace)
        {
            workspace.IsUntitled = false;
            workspace.FileName = filename;
        }

        #endregion // Static Accessors

        #region Fields

        private List<Block> _unattached = new List<Block>();
        internal StartBlock _startBlock = new StartBlock();

        #endregion // Fields

        #region Properties

        public string FileName { get; protected set; }

        public bool IsUntitled { get; protected set; }

        #endregion Properties

        #region Constructors

        private Workspace()
        {
        }

        #endregion // Constructors

        #region Methods

        public object Clone()
        {
            //Workspace clone = Workspace.CreateNewWorkspace();
            Workspace clone = new Workspace();

            clone._startBlock = _startBlock.Clone() as StartBlock;
            foreach (Block block in _unattached) {
                clone._unattached.Add(block.Clone() as Block);
            }

            clone.FileName = this.FileName; //immutable
            clone.IsUntitled = IsUntitled;

            return clone;
        }

        public void Serialise(Stream stream) {
            ModelSerialiser.Serialise(stream, this);
        }

        public static Workspace Deserialise(Stream stream) {
            return ModelSerialiser.Deserialise(stream) as Workspace;
        }
                
        #endregion
    }
}
