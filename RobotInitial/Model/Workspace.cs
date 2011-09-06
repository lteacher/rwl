using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Properties;
using System.Xml.Serialization;
using System.IO;

namespace RobotInitial.Model
{
    public class Workspace : ICloneable, IXmlSerializable
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
        private StartBlock _startBlock = new StartBlock();

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

        //public void Serialise(Stream stream) {
        //    XmlSerializer serialiser = new XmlSerializer(this.GetType());
        //    serialiser.Serialize(stream, this);
        //}

        public System.Xml.Schema.XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader) {
            throw new NotImplementedException();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            throw new NotImplementedException();
        }

        #endregion

 
    }
}
