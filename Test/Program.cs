using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Logger;
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
            BufferingLogger logger = new BufferingLogger(100, ".");
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

            serviceManager.Dispose();
            container.Dispose();
            logger.Dispose();

            Console.ReadLine();
        }
    }
}
