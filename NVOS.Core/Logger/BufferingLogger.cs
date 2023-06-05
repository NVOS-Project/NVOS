using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NVOS.Core.Logger
{
    public class BufferingLogger : ILogger, IDisposable
    {
        private LogBuffer<string> buffer;
        private DateTime logDate;
        private StreamWriter streamWriter;
        private LogLevel logLevel;
        private int fileIndex;
        private string filePath;

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                SetPath(value);
            }
        }

        public BufferingLogger(int bufferSize, string filePath)
        {
            buffer = new LogBuffer<string>(bufferSize);
            logDate = DateTime.Now;
            this.filePath = filePath;
            logLevel = LogLevel.INFO;
        }

        public BufferingLogger(int bufferSize, string filePath, LogLevel logLevel)
        {
            buffer = new LogBuffer<string>(bufferSize);
            logDate = DateTime.Now;
            this.filePath = filePath;
            this.logLevel = logLevel;
        }

        public bool Init()
        {
            SetPath(filePath);
            return true;
        }

        public void Dispose()
        {
            streamWriter.Close();
        }

        public void SetLevel(LogLevel level)
        {
            logLevel = level;
        }

        public LogLevel GetLevel()
        {
            return logLevel;
        }

        public void Log(LogLevel level, string message)
        {
            if (level < logLevel)
                return;

            buffer.Write(message);
            string logMessage = $"<{DateTime.Now}> [{level}] {message}";

            streamWriter.WriteLine(logMessage);
            streamWriter.Flush();
        }

        public void Debug(string message)
        {
            Log(LogLevel.DEBUG, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.INFO, message);
        }

        public void Warn(string message)
        {
            Log(LogLevel.WARN, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.ERROR, message);
        }

        public IEnumerable<string> ReadLogs()
        {
            foreach (string message in buffer.ReadItems())
            {
                yield return message;
            }
        }

        private void SetPath(string path)
        {
            fileIndex = 0;
            string logFileName = $"{logDate:MM-dd-yyyy}_{fileIndex}.log";
            string logFilePath = Path.Combine(path, logFileName);

            while (File.Exists(logFilePath))
            {
                fileIndex++;
                logFileName = $"{logDate:MM-dd-yyyy}_{fileIndex}.log";
                logFilePath = Path.Combine(path, logFileName);
            }

            filePath = logFilePath;
            if (streamWriter != null)
                streamWriter.Close();

            FileStream fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
            streamWriter = new StreamWriter(fileStream);
        }
    }
}
