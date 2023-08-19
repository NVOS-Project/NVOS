using NVOS.Core.Logger.Enums;

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
