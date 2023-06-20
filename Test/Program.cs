using NVOS.Core;
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
            Bootstrap.Init();
            IServiceManager serviceManager = ServiceLocator.Resolve<ServiceManager>();
            IModuleManager moduleManager = ServiceLocator.Resolve<ModuleManager>();
            Assembly testModule = Assembly.Load("TestModule");
            moduleManager.Load(testModule);
            serviceManager.Start<TestService>();
            serviceManager.Stop<TestService>();
            moduleManager.Unload(testModule);
            Console.ReadLine();
        }
    }
}
