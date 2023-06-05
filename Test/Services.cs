using NVOS.Core;
using NVOS.Core.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
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

    [ServiceDependency(typeof(ServiceD))]
    [ServiceDependency(typeof(ServiceE))]
    public class ServiceC : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service C up!");
            return true;
        }
    }

    public class ServiceD : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service D up!");
            return true;
        }
    }

    public class ServiceE : IService
    {
        public bool Init()
        {
            Console.WriteLine("Service E up!");
            return true;
        }
    }

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
