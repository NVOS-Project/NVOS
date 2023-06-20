using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Containers
{
    public class ManagedContainer : IDisposable
    {
        private IContainer container;
        private ILifetimeScope rootScope;

        public ManagedContainer(params ICoreComponentRegistry[] registries)
        {
            Init(registries);
        }

        private void Init(ICoreComponentRegistry[] registries)
        {
            ContainerBuilder builder = new ContainerBuilder();
            foreach (ICoreComponentRegistry registry in registries)
                registry.Register(builder);

            container = builder.Build();
            BeginRootScope();
        }

        public bool BeginRootScope()
        {
            if (container == null)
                throw new ObjectDisposedException(nameof(container));

            if (rootScope != null)
                return false;

            rootScope = container.BeginLifetimeScope();
            return true;
        }

        public bool EndRootScope()
        {
            if (container == null)
                throw new ObjectDisposedException(nameof(container));

            if (rootScope == null)
                return false;

            rootScope.Dispose();
            rootScope = null;
            return true;
        }

        public ILifetimeScope GetRootScope()
        {
            return rootScope;
        }

        public void Dispose()
        {
            EndRootScope();
            container?.Dispose();
            container = null;
        }
    }
}
