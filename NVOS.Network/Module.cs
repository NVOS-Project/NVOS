using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.UI
{
    public class Module : IModule
    {
        public string Name => "NVOS.Network";

        public string Description => "cool";

        public string Author => "g";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
