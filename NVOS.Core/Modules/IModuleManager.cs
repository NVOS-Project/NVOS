using NVOS.Core.Modules.EventArgs;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NVOS.Core.Modules
{
    public interface IModuleManager
    {
        event EventHandler<ModuleEventArgs> ModuleLoaded;
        event EventHandler<ModuleEventArgs> ModuleUnloaded;

        IModule Load(Assembly module);
        void Unload(Assembly module);
        bool IsLoaded(Assembly module);
        IEnumerable<KeyValuePair<Assembly, IModule>> GetLoadedModules();
    }
}