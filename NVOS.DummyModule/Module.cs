using NVOS.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.DummyModule
{
    public class Module : IModule
    {
        public string Name => "DummyModule";

        public string Description => "Module implementing services that spawn some Unity objects";

        public string Author => "NVOS Project";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
