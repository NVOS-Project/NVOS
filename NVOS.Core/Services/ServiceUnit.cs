using Autofac;
using NVOS.Core.Logger;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NVOS.Core.Services
{
    public class ServiceUnit
    {
        private ILogger logger;
        private ILifetimeScope serviceRootScope;
        private List<IService> instances;
        public Type DeclaringType { get; private set; }
        public string Domain { get; private set; }
        public ServiceType Type { get; private set; }
        public ServiceState State { get; private set; }
        public ServiceStopReason StopReason { get; private set; }
        public ILifetimeScope OwningScope { get; set; }

        public Assembly Assembly { get { return DeclaringType.Assembly; } }
        public string FullName { get { return DeclaringType.FullName; } }
        public string Name { get { return DeclaringType.Name; } }

        /// <summary>
        /// Instantiates a service unit controller tied to the given root scope
        /// </summary>
        /// <param name="rootScope">Root scope</param>
        /// <param name="declaringType">Service type</param>
        public ServiceUnit(ILogger logger, ILifetimeScope rootScope, Type declaringType, string domain = null)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            serviceRootScope = rootScope ?? throw new ArgumentNullException(nameof(rootScope));
            instances = new List<IService>();

            if (declaringType == null)
                throw new ArgumentNullException(nameof(declaringType));

            if (!(typeof(IService).IsAssignableFrom(declaringType) && declaringType.IsClass && !declaringType.IsAbstract))
                throw new ArgumentException("The specified type is not a service.");

            ServiceTypeAttribute attr = declaringType.GetCustomAttribute<ServiceTypeAttribute>();
            if (attr == null)
                throw new ArgumentException("The specified type does not declare a service type.");

            DeclaringType = declaringType;
            Type = attr.Type;
            State = ServiceState.Stopped;
            StopReason = ServiceStopReason.None;
            OwningScope = null;
            Domain = domain ?? declaringType.Assembly.FullName;
        }

        private void CreateServiceScope()
        {
            if (OwningScope != null)
                throw new InvalidOperationException("Service scope already exists.");

            ILifetimeScope scope = serviceRootScope.BeginLifetimeScope(builder =>
            {
                var registration = builder.RegisterType(DeclaringType).AsSelf().As<IService>();
                switch (Type)
                {
                    case ServiceType.Singleton:
                        registration.SingleInstance();
                        break;
                    case ServiceType.Instance:
                        registration.InstancePerDependency();
                        break;
                }
            });

            OwningScope = scope;
        }

        private void DestroyServiceScope()
        {
            if (OwningScope == null)
                throw new InvalidOperationException("Service scope does not currently exist.");

            instances.Clear();
            OwningScope.Dispose();
            OwningScope = null;
        }

        private IService GetOrCreateInstance()
        {
            IService instance = (IService)OwningScope.Resolve(DeclaringType);
            if (!instances.Contains(instance))
            {
                instance.Init();
                instances.Add(instance);
            }

            return instance;
        }

        private bool DestroyInstance(IService instance)
        {
            if (instances.Contains(instance))
            {
                try
                {
                    IDisposable disposableService = instance as IDisposable;
                    disposableService?.Dispose();
                }
                catch (Exception ex)
                {
                    logger.Warn($"[ServiceUnit] Failed to destroy service instance: {ex}");
                }

                instances.Remove(instance);
                return true;
            }

            return false;
        }

        public void Start()
        {
            if (State == ServiceState.Running)
                throw new InvalidOperationException("Service is already running.");

            CreateServiceScope();
            if (Type == ServiceType.Singleton)
            {
                // Create and start singleton now
                try
                {
                    GetOrCreateInstance();
                }
                catch (Exception ex)
                {
                    DestroyServiceScope();
                    StopReason = ServiceStopReason.Failure;
                    throw ex;
                }
            }

            State = ServiceState.Running;
            StopReason = ServiceStopReason.None;
        }

        public void Stop(ServiceStopReason reason = ServiceStopReason.Normal)
        {
            if (State == ServiceState.Stopped)
                throw new InvalidOperationException("Service is not running.");

            DestroyServiceScope();
            State = ServiceState.Stopped;
            StopReason = reason;
        }

        public int GetInstanceCount()
        {
            return instances.Count;
        }

        public IService Resolve()
        {
            if (State != ServiceState.Running)
                throw new InvalidOperationException("Service unit is not running.");

            if (OwningScope == null)
                throw new InvalidOperationException("Service scope does not exist.");

            return GetOrCreateInstance();
        }
    }
}
