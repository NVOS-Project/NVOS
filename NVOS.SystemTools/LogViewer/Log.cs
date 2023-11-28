using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NVOS.SystemTools.LogViewer
{
    public class Log
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }
        public string[] Tags { get; set; }

        public Log(LogLevel logLevel, string message, string[] tags)
        {
            Level = logLevel;
            Message = message;
            Tags = tags;
        }
    }
}
