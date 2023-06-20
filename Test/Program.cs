using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Database.Serialization;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Modules;
using NVOS.Core.Services;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Reflection;
using TestModule;

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
            ModuleManager moduleManager = new ModuleManager(logger, serviceManager);

            logger.SetLevel(LogLevel.INFO);

            Assembly testModule = Assembly.Load("TestModule");
            moduleManager.Load(testModule);
            serviceManager.Start<TestService>();
            serviceManager.Stop<TestService>();
            moduleManager.Unload(testModule);
            Console.ReadLine();
        }
    }
}
