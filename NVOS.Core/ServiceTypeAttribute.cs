using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core
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
