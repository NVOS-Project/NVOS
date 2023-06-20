using NVOS.Core.Services.Enums;
using NVOS.Core.Services.EventArgs;
using System;
using System.Collections.Generic;

namespace NVOS.Core.Services
{
    public interface IServiceManager : IServiceResolver, IDisposable
    {
        event EventHandler<ServiceEventArgs> ServiceRegistered;
        event EventHandler<ServiceEventArgs> ServiceUnregistered;
        event EventHandler<ServiceEventArgs> ServiceStarted;
        event EventHandler<ServiceEventArgs> ServiceStopped;

        void Start<T>() where T : IService;
        void Start(Type type);
        void Stop<T>() where T : IService;
        void Stop(Type type);
        void Register<T>(string domain = null) where T : IService;
        void Register(Type type, string domain = null);
        void Unregister<T>() where T : IService;
        void Unregister(Type type);
        bool IsRegistered<T>() where T : IService;
        bool IsRegistered(Type type);
        ServiceState GetServiceState<T>() where T : IService;
        ServiceState GetServiceState(Type type);
        ServiceStopReason GetStopReason<T>() where T : IService;
        ServiceStopReason GetStopReason(Type type);
        IEnumerable<Type> GetRegisteredTypes();
    }
}
