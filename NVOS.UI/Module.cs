using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.UI
{
    public class Module : IModule
    {
        public string Name => "NVOS.UI";

        public string Description => "ujaj";

        public string Author => "g";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
