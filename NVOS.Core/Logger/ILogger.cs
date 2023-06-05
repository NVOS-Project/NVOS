using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Logger
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        IEnumerable<string> ReadLogs();
    }
}
