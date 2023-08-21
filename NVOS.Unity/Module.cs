using NVOS.Core.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NVOS.Unity
{
    public class Module : IModule
    {
        public string Name => "NVOS.Unity";

        public string Description => "A common set of Unity utilities";

        public string Author => "NVOS Project";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
