using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Services;

namespace NVOS.Core.Registries
{
    public class ServiceRegistry : ICoreComponentRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceManager>().AsSelf().As<IServiceManager>().As<IServiceResolver>().SingleInstance();
            builder.RegisterType<ServiceDependencyResolver>().InstancePerDependency();
        }
    }
}
