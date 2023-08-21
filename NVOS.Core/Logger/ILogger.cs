using NVOS.Core.Logger.Enums;
using NVOS.Core.Logger.EventArgs;
using System;
using System.Collections.Generic;

namespace NVOS.Core.Logger
{
    public interface ILogger
    {
        event EventHandler<LogEventArgs> OnLog;
        void Log(LogLevel level, string message);
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        LogLevel GetLevel();
        void SetLevel(LogLevel level);
        IEnumerable<string> ReadLogs();
    }
}
