using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
namespace RobotInitial.Services
{
    class StreamService : IStreamService
    {
        public FileStream GetReadableFileStream(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            return fs;
        }

        public FileStream GetWriteableFileStream(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            return fs;
        }
    }
}
