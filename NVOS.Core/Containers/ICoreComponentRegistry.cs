using Autofac;

namespace NVOS.Core.Containers
{
    public interface ICoreComponentRegistry
    {
        void Register(ContainerBuilder builder);
    }
}
