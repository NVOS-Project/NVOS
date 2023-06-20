using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Modules;

namespace NVOS.Core.Registries
{
    public class ModuleRegistry : ICoreComponentRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ModuleManager>().AsSelf().As<IModuleManager>().SingleInstance();
        }
    }
}
