using NVOS.Core.Services.EventArgs;
using System;

namespace NVOS.Core.Services
{
    public interface IServiceManager : IServiceResolver
    {
        event EventHandler<ServiceEventArgs> OnServiceRegistered;
        event EventHandler<ServiceEventArgs> OnServiceUnregistered;
        event EventHandler<ServiceEventArgs> OnServiceStarted;
        event EventHandler<ServiceEventArgs> OnServiceStopped;

        void Start<T>();
        void Start(Type type);
        void StartDomain(string domain);
        void Stop<T>();
        void Stop(Type type);
        void StopDomain(string domain);
        void Register<T>(string domain = null);
        void Register(Type type, string domain = null);
        void RegisterDomain(string domain, params Type[] types);
        void Unregister<T>();
        void Unregister(Type type);
        void UnregisterDomain(string domain);
    }
}
