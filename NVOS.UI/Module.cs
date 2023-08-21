using NVOS.Core.Modules;
using System;
using System.Reflection;

namespace NVOS.UI
{
    public class Module : IModule
    {
        public string Name => "NVOS.UI";

        public string Description => "A library implementing UI systems in Unity";

        public string Author => "NVOS Project";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
