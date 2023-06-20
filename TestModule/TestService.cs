using NVOS.Core;
using NVOS.Core.Services.Attributes;
using System;

namespace TestModule
{
    [ServiceType(NVOS.Core.Services.Enums.ServiceType.Singleton)]
    public class TestService : IService
    {
        public bool Init()
        {
            Console.WriteLine("Fun!");
            return true;
        }
    }
}
