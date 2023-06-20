using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Modules.EventArgs
{
    public class ModuleEventArgs : System.EventArgs
    {
        public Assembly Assembly;
        public IModule Manifest;

        public ModuleEventArgs(Assembly assembly, IModule manifest)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            Manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        }
    }
}
