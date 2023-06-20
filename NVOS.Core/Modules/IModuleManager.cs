using NVOS.Core.Modules.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Modules
{
    public interface IModuleManager
    {
        event EventHandler<ModuleEventArgs> ModuleLoaded;
        event EventHandler<ModuleEventArgs> ModuleUnloaded;

        IModule Load(Assembly module);
        void Unload(Assembly module);
        IEnumerable<KeyValuePair<Assembly, IModule>> GetLoadedModules();
    }
}