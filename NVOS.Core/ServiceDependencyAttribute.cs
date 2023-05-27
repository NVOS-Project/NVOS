using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ServiceDependencyAttribute : Attribute
    {
        private string name;
        private Type type;

        public ServiceDependencyAttribute(string serviceName)
        {
            name = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
        }

        public ServiceDependencyAttribute(Type serviceType)
        {
            type = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        }

        public Type Resolve()
        {
            if (type != null)
                return type;

            type = Type.GetType(name);
            return type;
        }
    }
}
