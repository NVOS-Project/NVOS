using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Logger
{
    public class LoggerService : ILogger
    {
        private LogBuffer<string> buffer;
        private DateTime logDate;
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

        public LoggerService(int bufferSize, string filePath)
        {
            buffer = new LogBuffer<string>(bufferSize);
            logDate = DateTime.Now;
            FilePath = filePath;
            fileIndex = 0;
        }

        public bool Init()
        {
            return true;
        }   

        public void Log(string message, LogLevel level = LogLevel.INFO)
        {
            buffer.Write(message);
            string logMessage = $"<{DateTime.Now}> [{level}] {message}\n";

            File.AppendAllText(FilePath, logMessage);
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

            Console.WriteLine(logDate);

            filePath = logFilePath;
        }
    }
}
