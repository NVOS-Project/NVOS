using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.UI
{
    public class Module : IModule
    {
        public string Name => "NVOS.LED";

        public string Description => "UI for controlling the night vision LED array";

        public string Author => "NVOS Project";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
