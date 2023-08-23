using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.SystemTools
{
    public class Module : IModule
    {
        public string Name => "NVOS.SystemTools";

        public string Description => "many tool (very important)";

        public string Author => "g";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
