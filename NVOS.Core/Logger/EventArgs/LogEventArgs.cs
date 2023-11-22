using NVOS.Core.Logger.Enums;

namespace NVOS.Core.Logger.EventArgs
{
    public class LogEventArgs : System.EventArgs
    {
        public LogLevel Level;
        public string Message;
        public string[] Tags;

        public LogEventArgs(LogLevel level, string message, string[] tags)
        {
            Level = level;
            Message = message;
            Tags = tags;
        }
    }
}
