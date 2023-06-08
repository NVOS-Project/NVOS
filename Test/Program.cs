using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Database.Serialization;
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
            JsonDbValueSerializer serializer = new JsonDbValueSerializer();
            LiteDbService database = new LiteDbService(serializer);
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

            logger.Info("i on został poinformowany");
            logger.Info("i on został poinformowany");
            logger.Debug("i on został zdebugowany");

            database.Write("logger", "logDirectory", "logs2");

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
