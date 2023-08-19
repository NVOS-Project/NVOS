using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Logger.EventArgs
{
    public class LogEventArgs : System.EventArgs
    {
        public LogLevel Level;
        public string Message;

        public LogEventArgs(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }
    }
}
