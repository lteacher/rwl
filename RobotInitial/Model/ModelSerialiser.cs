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

        //appends onto the end of the stream.
        internal static void serialise(Stream stream, object ob) {
            stream.Position = stream.Length;
            try {
                formatter.Serialize(stream, ob);
            } catch (SerializationException e) {
                Debug.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            //let the calling classes handle the stream itself
            //stream.Flush(); 
        }

        //deserialises from the start of the stream
        internal static object deserialise(Stream stream) {
            stream.Position = 0;
            object ob;

            try {
                ob = formatter.Deserialize(stream);
            } catch (SerializationException e) {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }

            return ob;
        }
    }
}
