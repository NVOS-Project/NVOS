using Autofac;

namespace NVOS.Core.Containers
{
    public interface IComponentRegistry
    {
        void Register(ContainerBuilder builder);
    }
}
