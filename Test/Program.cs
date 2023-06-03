using NVOS.Core.Database;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using System;
using System.Collections.Generic;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            LoggerService logger = new LoggerService(5, "./");

            logger.Log("debug log 1", LogLevel.DEBUG);
            logger.Log("debug log 2", LogLevel.DEBUG);
            logger.Log("debug log 3", LogLevel.DEBUG);
            logger.Log("fatal log 1", LogLevel.FATAL);
            logger.Log("info log 1");
            logger.Log("info log 2");

            foreach (string message in logger.ReadLogs())
            {
                Console.WriteLine(message);
            }

            Console.ReadLine();
        }
    }
}
