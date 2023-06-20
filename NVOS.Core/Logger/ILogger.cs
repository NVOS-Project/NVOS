using NVOS.Core.Logger.Enums;
using System.Collections.Generic;

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
