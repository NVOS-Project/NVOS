using NVOS.Core.Logger.Enums;
using NVOS.Core.Logger.EventArgs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NVOS.Core.Logger
{
    public interface ILogger
    {
        event EventHandler<LogEventArgs> OnLog;
        void Debug(string message, string optionalTag = null);
        void Info(string message, string optionalTag = null);
        void Warn(string message, string optionalTag = null);
        void Error(string message, string optionalTag = null);
        LogLevel GetLevel();
        void SetLevel(LogLevel level);
        IEnumerable<string> ReadLogs();
    }
}
