using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace RobotInitial.Model {
    internal class ModelSerialiser {
        private static BinaryFormatter formatter = new BinaryFormatter();

        private ModelSerialiser() {
        }

        internal static void Serialise(Stream stream, object ob) {
            try {
                formatter.Serialize(stream, ob);
            } catch (SerializationException e) {
                Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            //let the calling classes handle the stream itself
            //stream.Flush(); 
        }

        internal static object Deserialise(Stream stream) {
            object ob;

            try {
                ob = formatter.Deserialize(stream);
            } catch (SerializationException e) {
                Debug.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }

            return ob;
        }
    }
}
