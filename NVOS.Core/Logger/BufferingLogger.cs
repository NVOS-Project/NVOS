﻿using NVOS.Core.Database;
using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircularBuffer;

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

        public BufferingLogger(IDatabaseService database)
        {
            collection = database.GetCollection("logger");
            int bufferSize = (int)collection.ReadOrDefault("bufferSize", 200);
            logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), (string)collection.ReadOrDefault("logLevel", LogLevel.INFO));
            logDirectory = (string)collection.ReadOrDefault("logDirectory", "logs");
            buffer = new CircularBuffer<string>(bufferSize);
            Init();
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

        public void Log(LogLevel level, string message)
        {
            if (level < logLevel)
                return;

            string logMessage = $"[{DateTime.Now}] <{level}> {message}";

            Console.WriteLine(logMessage);
            buffer.PushBack(logMessage);
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
