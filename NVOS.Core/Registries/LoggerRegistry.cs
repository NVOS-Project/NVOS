using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Logger;

namespace NVOS.Core.Registries
{
    public class LoggerRegistry : ICoreComponentRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<BufferingLogger>().AsSelf().As<ILogger>().SingleInstance();
        }
    }
}
