using NVOS.Core.Logger;
using NVOS.Core.Modules.EventArgs;
using NVOS.Core.Modules.Extensions;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NVOS.Core.Modules
{
    public class ModuleManager : IModuleManager
    {
        private ILogger logger;
        private IServiceManager serviceManager;
        private Dictionary<Assembly, IModule> loadedModules;

        public event EventHandler<ModuleEventArgs> ModuleLoaded;
        public event EventHandler<ModuleEventArgs> ModuleUnloaded;

        public ModuleManager(ILogger logger, IServiceManager serviceManager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            loadedModules = new Dictionary<Assembly, IModule>();
        }

        private void LoadDependencies(Assembly assembly)
        {
            logger.Info($"[ModuleManager] Resolving dependencies for module {assembly.FullName}");
            IEnumerable<AssemblyName> dependencies = assembly.GetReferencedAssemblies();
            foreach (AssemblyName dependency in dependencies)
            {
                Assembly dependencyAssembly = Assembly.Load(dependency);
                if (loadedModules.ContainsKey(dependencyAssembly))
                {
                    logger.Debug($"[ModuleManager] Skipping already resolved module {assembly.FullName}");
                    continue;
                }

                if (!dependencyAssembly.HasModuleManifest())
                {
                    logger.Debug($"[ModuleManager] Assembly {assembly.FullName} is not a module, skipping.");
                    continue;
                }

                Load(dependencyAssembly);
            }
        }

        private void RegisterServices(Assembly assembly)
        {
            logger.Info($"[ModuleManager] Registering services for module {assembly.FullName}");

            IEnumerable<Type> services = assembly.GetModuleServices();
            foreach (Type service in services)
            {
                try
                {
                    serviceManager.Register(service);
                }
                catch (Exception ex)
                {
                    logger.Error($"[ModuleManager] Failed to register service {service.FullName}: {ex}");
                    logger.Info("[ModuleManager] Starting registration rollback");
                    UnregisterServices(assembly);
                    logger.Info("[ModuleManager] Registrations rolled back");
                    throw;
                }
            }

            logger.Info($"[ModuleManager] Registered {services.Count()} services");
        }

        private void UnregisterServices(Assembly assembly)
        {
            logger.Info($"[ModuleManager] Unregistering services for module {assembly.FullName}");
            foreach (Type service in assembly.GetModuleServices())
            {
                try
                {
                    if (serviceManager.IsRegistered(service))
                        serviceManager.Unregister(service);
                }
                catch (Exception ex)
                {
                    logger.Warn($"[ModuleManager] Failed to unregister service {service.FullName}: {ex}");
                }
            }
        }

        private void StartServices(Assembly assembly)
        {
            logger.Info($"[ModuleManager] Starting all services for module {assembly.FullName}");
            foreach (Type service in assembly.GetModuleServices())
            {
                try
                {
                    serviceManager.Start(service);
                }
                catch (Exception ex)
                {
                    logger.Warn($"[ModuleManager] Failed to start service {service.FullName}: {ex}");
                }
            }
        }

        private void StopServices(Assembly assembly)
        {
            logger.Info($"[ModuleManager] Stopping all services for module {assembly.FullName}");
            foreach (Type service in assembly.GetModuleServices())
            {
                try
                {
                    serviceManager.Stop(service);
                }
                catch (Exception ex)
                {
                    logger.Warn($"[ModuleManager] Failed to stop service {service.FullName}: {ex}");
                }
            }
        }

        public IModule Load(Assembly assembly)
        {
            if (loadedModules.ContainsKey(assembly))
                throw new InvalidOperationException("Module is already loaded.");

            IModule manifest = assembly.GetModuleManifest();
            LoadDependencies(assembly);

            RegisterServices(assembly);
            StartServices(assembly);

            logger.Info($"[ModuleManager] Module {manifest.Name} ({assembly.FullName}) loaded");
            logger.Info($"[ModuleManager] {manifest.Name} version {manifest.Version} by {manifest.Author}");
            logger.Info($"[ModuleManager] {manifest.Description}");

            loadedModules.Add(assembly, manifest);
            ModuleLoaded?.Invoke(this, new ModuleEventArgs(assembly, manifest));

            return manifest;
        }

        public void Unload(Assembly assembly)
        {
            if (!loadedModules.ContainsKey(assembly))
                throw new InvalidOperationException("Module is not loaded.");

            IModule manifest = loadedModules[assembly];

            StopServices(assembly);
            UnregisterServices(assembly);

            logger.Info($"[ModuleManager] Module {manifest.Name} ({assembly.FullName}) unloaded.");

            loadedModules.Remove(assembly);
            ModuleUnloaded?.Invoke(this, new ModuleEventArgs(assembly, manifest));
        }

        public IEnumerable<KeyValuePair<Assembly, IModule>> GetLoadedModules()
        {
            return loadedModules;
        }
    }
}
