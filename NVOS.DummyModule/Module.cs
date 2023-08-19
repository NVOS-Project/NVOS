using NVOS.Core.Modules;
using System;
using System.Reflection;

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
