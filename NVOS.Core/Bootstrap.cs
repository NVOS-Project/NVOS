using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Registries;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;

namespace NVOS.Core
{
    public static class Bootstrap
    {
        private static ManagedContainer container;
        public static bool IsInitialized { get; private set; }
        public const string DEFAULT_DB_PATH = "database.db";

        public static void Init(ICoreComponentRegistry[] additionalRegistries = null)
        {
            Init(DEFAULT_DB_PATH, additionalRegistries);
        }
        public static void Init(string dbPath, ICoreComponentRegistry[] additionalRegistries = null)
        {
            if (IsInitialized)
                throw new InvalidOperationException("The core is already initialized.");

            List<ICoreComponentRegistry> registries = new List<ICoreComponentRegistry>();

            // Register core components
            registries.Add(new LiteDbRegistry(dbPath));
            registries.Add(new LoggerRegistry());
            registries.Add(new ModuleRegistry());
            registries.Add(new ServiceRegistry());

            // Register additional components
            if (additionalRegistries != null)
                registries.AddRange(additionalRegistries);

            container = new ManagedContainer(registries.ToArray());
            ILifetimeScope scope = container.GetRootScope();
            IServiceResolver resolver = scope.Resolve<IServiceResolver>();
            ServiceLocator.SetResolver(resolver);
            IsInitialized = true;
        }
    }
}
