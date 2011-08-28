using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace RobotInitial.Services
{
    interface IStreamService
    {
        FileStream GetReadableFileStream(string filename);
        FileStream GetWriteableFileStream(string filename);
    }
}
