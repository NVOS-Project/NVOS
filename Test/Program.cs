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
            BufferingLogger logger = new BufferingLogger(5, "./", LogLevel.INFO);
            logger.Init();

            logger.Debug("debug log 1");
            logger.Debug("debug log 2");
            logger.Debug("debug log 3");
            logger.Error("error log 1");
            logger.Info("info log 1");
            logger.Info("info log 2");

            foreach (string message in logger.ReadLogs())
            {
                Console.WriteLine(message);
            }

            Console.ReadLine();
        }
    }
}
