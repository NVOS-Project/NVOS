using System;

namespace NVOS.Core.Services
{
    public interface IServiceResolver
    {
        T Resolve<T>() where T : class;
        object Resolve(Type type);
        T ResolveOptional<T>() where T : class;
        object ResolveOptional(Type type);
    }
}
