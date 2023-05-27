using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InjectDependencyAttribute : Attribute
    {
    }
}
