using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using QuikGraph;
using System;
using System.Collections.Generic;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            LiteDbService database = new LiteDbService();
            database.Open("./database.db");
            BufferingLogger logger = new BufferingLogger(database);
            ManagedContainer container = new ManagedContainer();
            ServiceDependencyResolver resolver = new ServiceDependencyResolver();
            ServiceManager serviceManager = new ServiceManager(container, resolver, logger);
            serviceManager.Register<ServiceA>();
            serviceManager.Register<ServiceB>();
            serviceManager.Register<ServiceC>();
            serviceManager.Register<ServiceD>();
            serviceManager.Register<ServiceE>();
            serviceManager.Register<ServiceF>();

            serviceManager.Start<ServiceF>();

            logger.SetLevel(LogLevel.WARN);
            logger.Info("i on został poinformowany");
            logger.Info("i on został poinformowany");
            logger.Debug("i on został zdebugowany");
            logger.Warn("i on został ostrzeżony");
            logger.Error("i on został zabłądzony");

            foreach (string log in logger.ReadLogs())
            {
                Console.WriteLine(log);
            }

            serviceManager.Dispose();
            container.Dispose();
            logger.Dispose();

            Console.ReadLine();
        }
    }
}
