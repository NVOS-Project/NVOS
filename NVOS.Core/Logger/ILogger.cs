using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Logger
{
    public interface ILogger : ICoreService
    {
        string FilePath { get; set; }
        void Log(string message, LogLevel level = LogLevel.INFO);
        IEnumerable<string> ReadLogs();
    }
}
