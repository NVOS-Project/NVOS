using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.UI
{
    public class Module : IModule
    {
        public string Name => "NVOS.Network";

        public string Description => "A library implementing connectivity to NVOS Embedded instances";

        public string Author => "NVOS Project";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
