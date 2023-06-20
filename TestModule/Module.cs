using NVOS.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class Module : IModule
    {
        public string Name => "TestModule";

        public string Description => "Test module for testing fun things";

        public string Author => "Me";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
