using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using System;

namespace Test
{
    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(ServiceB))]
    [ServiceDependency(typeof(ServiceD))]
    public class ServiceA : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service A up!");
            return true;
        }
    }

    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(ServiceC))]
    [ServiceDependency(typeof(ServiceE))]
    public class ServiceB : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service B up!");
            return true;
        }
    }

    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(ServiceD))]
    [ServiceDependency(typeof(ServiceE))]
    public class ServiceC : IService, IDisposable
    {
        public bool Init()
        {
            Console.WriteLine("Service C up!");
            return true;
        }

        public void Dispose()
        {
            Console.WriteLine("Dobranoc C");
        }
    }

    [ServiceType(ServiceType.Singleton)]
    public class ServiceD : IService, IDisposable
    {
        public bool Init()
        {
            Console.WriteLine("Service D up!");
            return true;
        }

        public void Dispose()
        {
            Console.WriteLine("Dobranoc D");
        }
    }

    [ServiceType(ServiceType.Singleton)]
    public class ServiceE : IService, IDisposable
    {
        public bool Init()
        {
            Console.WriteLine("Service E up!");
            return true;
        }

        public void Dispose()
        {
            Console.WriteLine("Dobranoc E");
        }
    }

    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(ServiceA))]
    public class ServiceF : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service F up!");
            return true;
        }
    }
}
