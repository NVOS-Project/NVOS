using System;
using System.Reflection;

namespace NVOS.Core.Services.EventArgs
{
    public class ServiceEventArgs
    {
        public Type DeclaringType;
        public string Domain;

        public ServiceEventArgs(Type declaringType, string domain)
        {
            DeclaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }

        public Assembly Assembly { get { return DeclaringType.Assembly; } }
        public string FullName { get { return DeclaringType.FullName; } }
        public string Name { get { return DeclaringType.Name; } }
    }
}
