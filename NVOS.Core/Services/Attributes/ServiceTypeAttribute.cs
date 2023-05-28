using NVOS.Core.Services.Enums;
using System;

namespace NVOS.Core.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ServiceTypeAttribute : Attribute
    {
        public ServiceType Type { get; private set; }
        public ServiceTypeAttribute(ServiceType type)
        {
            Type = type;
        }
    }
}
