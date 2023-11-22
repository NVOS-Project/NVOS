using CircularBuffer;
using NVOS.Core.Database;
using NVOS.Core.Database.EventArgs;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Logger.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NVOS.Core.Logger
{
    public class BufferingLogger : ILogger, IDisposable
    {
        private CircularBuffer<string> buffer;
        private StreamWriter streamWriter;
        private LogLevel logLevel;
        private DbCollection collection;
        private int fileIndex;
        private string logDirectory;
        private string filePath;

        public event EventHandler<LogEventArgs> OnLog;

        public BufferingLogger(IDatabaseService database)
        {
            collection = database.GetCollection("logger");
            int bufferSize = (int)collection.ReadOrDefault("bufferSize", 200);
            logLevel = (LogLevel)collection.ReadOrDefault("logLevel", LogLevel.INFO);
            logDirectory = (string)collection.ReadOrDefault("logDirectory", "Logs");
            buffer = new CircularBuffer<string>(bufferSize);
            collection.RecordWritten += Collection_RecordWritten;
            Init();
        }

        private void Collection_RecordWritten(object sender, DbRecordEventArgs e)
        {
            if (e.Key == "logDirectory")
            {
                logDirectory = (string)collection.Read(e.Key);
                Init();
            }
        }

        private void Init()
        {
            DateTime logDate = DateTime.Now;

            string logFileName = $"{logDate:MM-dd-yyyy}_{fileIndex}.log";
            string logFilePath = Path.Combine(logDirectory, logFileName);

            while (File.Exists(logFilePath))
            {
                fileIndex++;
                logFileName = $"{logDate:MM-dd-yyyy}_{fileIndex}.log";
                logFilePath = Path.Combine(logDirectory, logFileName);
            }

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            filePath = logFilePath;
            FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            streamWriter = new StreamWriter(fileStream);
        }

        public void Dispose()
        {
            streamWriter.Close();
        }

        public void SetLevel(LogLevel level)
        {
            logLevel = level;
            collection.Write("logLevel", level);
        }

        public LogLevel GetLevel()
        {
            return logLevel;
        }

        private void Log(LogLevel level, string message, string optionalTag = null)
        {
            if (level < logLevel)
                return;

            string[] tags = new string[] {};

            Type caller = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().DeclaringType;

            if (optionalTag != null)
                tags.Append(optionalTag);
            tags.Append(caller.Name);

            string logMessage = $"[{DateTime.Now}] <{level}> {message}";

            Console.WriteLine(logMessage);
            buffer.PushBack(logMessage);
            streamWriter.WriteLine(logMessage);
            streamWriter.Flush();
            OnLog?.Invoke(this, new LogEventArgs(level, logMessage, tags));
        }

        public void Debug(string message, string optionalTag = null)
        {
            Log(LogLevel.DEBUG, message, optionalTag);
        }

        public void Info(string message, string optionalTag = null)
        {
            Type caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            Log(LogLevel.INFO, message, optionalTag);
        }

        public void Warn(string message, string optionalTag = null)
        {
            Type caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            Log(LogLevel.WARN, message, optionalTag);
        }

        public void Error(string message, string optionalTag = null)
        {
            Type caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().DeclaringType;
            Log(LogLevel.ERROR, message, optionalTag);
        }

        public IEnumerable<string> ReadLogs()
        {
            foreach (string message in buffer)
            {
                yield return message;
            }
        }

        private void SetLogDirectory(string dir)
        {
            logDirectory = dir;
            collection.Write("logDirectory", dir);
            Init();
        }
    }
}
