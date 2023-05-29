using Autofac;

namespace NVOS.Core.Services
{
    public interface IServiceRegistry
    {
        void RegisterServices(ContainerBuilder builder);
    }
}
