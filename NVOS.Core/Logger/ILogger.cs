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
        void Log(LogLevel level, string message);
        IEnumerable<string> ReadLogs();
    }
}
