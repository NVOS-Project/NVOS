using NVOS.Core;
using NVOS.Core.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
