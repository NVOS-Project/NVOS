using NVOS.Core.Services.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Modules.Extensions
{
    public static class ModuleExtensions
    {
        public static bool HasModuleManifest(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).Any();
        }

        public static IModule GetModuleManifest(this Assembly assembly)
        {
            Type[] moduleTypes = assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface).ToArray();
            if (moduleTypes.Length == 0)
                throw new InvalidOperationException($"Assembly '{assembly.FullName}' does not contain a module manifest.");
            if (moduleTypes.Length != 1)
                throw new InvalidOperationException($"The module assembly '{assembly.FullName}' must contain exactly one implementation of IModule.");

            Type moduleType = moduleTypes[0];
            IModule module = Activator.CreateInstance(moduleType) as IModule;
            if (module == null)
                throw new InvalidOperationException($"Failed to create an instance of the module '{moduleType.FullName}'.");

            return module;
        }

        public static IEnumerable<Type> GetModuleServices(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(IService).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
        }
    }
}
