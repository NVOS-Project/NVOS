using System;

namespace NVOS.Core.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ServiceDependencyAttribute : Attribute
    {
        private bool required;
        private Type type;
        public bool IsRequired { get { return required; } }
        public Type Type { get { return type; } }

        public ServiceDependencyAttribute(Type serviceType, bool required = true)
        {
            this.required = required;
            type = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        }
    }
}
