using Autofac;
using Autofac.Core;
using NVOS.Core.Containers;
using NVOS.Core.Logger;
using NVOS.Core.Services.Enums;
using NVOS.Core.Services.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVOS.Core.Services
{
    public class ServiceManager : IServiceManager
    {
        private bool isDisposed;
        private ServiceDependencyResolver resolver;
        private Dictionary<Type, ServiceUnit> serviceUnits;
        private ILifetimeScope rootScope;
        private ILifetimeScope serviceScope;
        private ILogger logger;

        public event EventHandler<ServiceEventArgs> ServiceRegistered;
        public event EventHandler<ServiceEventArgs> ServiceUnregistered;
        public event EventHandler<ServiceEventArgs> ServiceStarted;
        public event EventHandler<ServiceEventArgs> ServiceStopped;

        public ServiceManager(ManagedContainer container, ServiceDependencyResolver resolver, ILogger logger)
        {
            serviceUnits = new Dictionary<Type, ServiceUnit>();
            this.resolver = resolver;
            this.logger = logger;

            rootScope = container.GetRootScope();
            rootScope.CurrentScopeEnding += RootScope_CurrentScopeEnding;
            serviceScope = rootScope.BeginLifetimeScope();
        }

        private void RootScope_CurrentScopeEnding(object sender, Autofac.Core.Lifetime.LifetimeScopeEndingEventArgs e)
        {
            Dispose();
        }

        private void AssertChainDependenciesMet(List<Type> chain)
        {
            foreach (Type type in chain)
            {
                if (!serviceUnits.ContainsKey(type))
                    throw new InvalidOperationException($"Missing dependency: {type.FullName}");
            }
        }

        private void StartUnit(Type type)
        {
            ServiceUnit unit = serviceUnits[type];
            if (unit.State == ServiceState.Running)
                return;

            logger.Info($"[ServiceManager] Starting service {type.Name}");
            unit.Start();
            logger.Info($"[ServiceManager] Service {type.Name} init finished");
            ServiceStarted?.Invoke(this, new ServiceEventArgs(type, unit.Domain));
        }

        private void StopUnit(Type type, ServiceStopReason reason = ServiceStopReason.Normal)
        {
            ServiceUnit unit = serviceUnits[type];
            if (unit.State == ServiceState.Stopped)
                return;

            logger.Info($"[ServiceManager] Stopping service {type.Name}");
            unit.Stop(reason);
            logger.Info($"[ServiceManager] Service {type.Name} shutdown finished");
            ServiceStopped?.Invoke(this, new ServiceEventArgs(type, unit.Domain));
        }

        private void StartChain(Type type)
        {
            List<Type> chain = resolver.ResolveDependencyOrder(type);
            List<Type> started = new List<Type>();
            AssertChainDependenciesMet(chain);
            foreach (Type dependency in chain)
            {
                try
                {
                    StartUnit(dependency);
                }
                catch (Exception ex)
                {
                    logger.Error($"[ServiceManager] Service chain start failed. Unit {dependency.Name} reported an error: {ex}");
                    break;
                }

                started.Add(dependency);
            }

            if (chain.Count != started.Count)
            {
                // Error must have occurred
                foreach (Type dependency in started.Reverse<Type>())
                {
                    try
                    {
                        StopUnit(dependency, ServiceStopReason.Failure);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn($"[ServiceManager] Stopping aborted chain start failed. Unit {dependency.Name} reported an error: {ex}");
                        break;
                    }
                }
            }
        }

        private void StopChain(Type type, ServiceStopReason reason = ServiceStopReason.Normal)
        {
            List<Type> chain = resolver.ResolveInverseDependencyOrder(type);
            foreach (Type dependency in chain)
            {
                if (!serviceUnits.ContainsKey(dependency))
                    continue;

                try
                {
                    if (dependency == type)
                        StopUnit(dependency, reason);
                    else
                        StopUnit(dependency, ServiceStopReason.Dependency);
                }
                catch (Exception ex)
                {
                    logger.Warn($"[ServiceManager] Unit {dependency.Name} reported an error while stopping {type.Name}'s chain: {ex}");
                }
            }
        }

        public IEnumerable<Type> GetRegisteredTypes()
        {
            foreach (Type type in serviceUnits.Keys)
                yield return type;
        }

        public void Register<T>(string domain = null) where T : IService
        {
            Register(typeof(T), domain);
        }

        public void Register(Type type, string domain = null)
        {
            if (serviceUnits.ContainsKey(type))
                throw new InvalidOperationException($"Service type {type.Name} already registered");

            ServiceUnit unit = new ServiceUnit(serviceScope, type, domain);
            serviceUnits.Add(type, unit);
            resolver.Register(type);
            logger.Info($"[ServiceManager] Service {type.Name} registered");
            ServiceRegistered?.Invoke(this, new ServiceEventArgs(type, domain));
        }

        public void Unregister<T>() where T : IService
        {
            Unregister(typeof(T));
        }

        public void Unregister(Type type)
        {
            if (!serviceUnits.ContainsKey(type))
                throw new KeyNotFoundException($"Service type {type.Name} is not registered");

            ServiceUnit unit = serviceUnits[type];
            if (unit.State == ServiceState.Running)
                throw new InvalidOperationException($"Service cannot be unregistered while running.");

            serviceUnits.Remove(type);
            resolver.Unregister(type);
            logger.Info($"[ServiceManager] Service {type.Name} unregistered");
            ServiceUnregistered?.Invoke(this, new ServiceEventArgs(type, unit.Domain));
        }

        public bool IsRegistered<T>() where T : IService
        {
            return IsRegistered(typeof(T));
        }

        public bool IsRegistered(Type type)
        {
            return serviceUnits.ContainsKey(type);
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(Type type)
        {
            if (serviceUnits.ContainsKey(type))
            {
                ServiceUnit service = serviceUnits[type];
                try
                {
                    return service.Resolve();
                }
                catch (Exception e)
                {
                    throw new DependencyResolutionException($"Failed to resolve dynamic dependency {type.Name}", e);
                }
            }

            logger.Debug($"[ServiceManager] Type {type.Name} resolved");
            return rootScope.Resolve(type);
        }

        public T ResolveOptional<T>() where T : class
        {
            return (T)ResolveOptional(typeof(T));
        }

        public object ResolveOptional(Type type)
        {
            try
            {
                if (serviceUnits.ContainsKey(type))
                {
                    ServiceUnit service = serviceUnits[type];
                    return service.Resolve();
                }

                return rootScope.Resolve(type);
            }
            catch
            {
                return null;
            }
        }

        public void Start<T>() where T : IService
        {
            Start(typeof(T));
        }

        public void Start(Type type)
        {
            if (!serviceUnits.ContainsKey(type))
                throw new KeyNotFoundException($"Service type {type.Name} is not registered");

            StartChain(type);
        }

        public void Stop<T>() where T : IService
        {
            Stop(typeof(T));
        }

        public void Stop(Type type)
        {
            if (!serviceUnits.ContainsKey(type))
                throw new KeyNotFoundException($"Service type {type.Name} is not registered");

            StopChain(type);
        }

        public ServiceState GetServiceState<T>() where T : IService
        {
            return GetServiceState(typeof(T));
        }

        public ServiceState GetServiceState(Type type)
        {
            if (!serviceUnits.ContainsKey(type))
                throw new KeyNotFoundException($"Service type {type.Name} is not registered");

            ServiceUnit unit = serviceUnits[type];
            return unit.State;
        }

        public ServiceStopReason GetStopReason<T>() where T : IService
        {
            return GetStopReason(typeof(T));
        }

        public ServiceStopReason GetStopReason(Type type)
        {
            if (!serviceUnits.ContainsKey(type))
                throw new KeyNotFoundException($"Service type {type.Name} is not registered");

            ServiceUnit unit = serviceUnits[type];
            return unit.StopReason;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            foreach (Type type in serviceUnits.Keys)
            {
                try
                {
                    StopUnit(type, ServiceStopReason.Shutdown);
                }
                catch (Exception ex)
                {
                    logger.Warn($"Failed to gracefully stop service {type.Name} while shutting down service manager: {ex}");
                }
            }

            serviceUnits.Clear();
            serviceScope?.Dispose();
            isDisposed = true;
        }
    }
}
