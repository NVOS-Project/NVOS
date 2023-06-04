using NVOS.Core.Database;
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
            ServiceDependencyResolver graphResolver = new ServiceDependencyResolver(typeof(ServiceA), typeof(ServiceB), typeof(ServiceC), typeof(ServiceD), typeof(ServiceE), typeof(ServiceF));
            Console.WriteLine("----------- ServiceA START CHAIN");
            foreach(Type type in graphResolver.ResolveDependencyOrder(typeof(ServiceA)))
            {
                Console.WriteLine(type.Name);
            }

            Console.WriteLine("----------- ServiceC STOP CHAIN");
            foreach (Type type in graphResolver.ResolveInverseDependencyOrder(typeof(ServiceC)))
            {
                Console.WriteLine(type.Name);
            }

            Console.ReadLine();
        }
    }
}
