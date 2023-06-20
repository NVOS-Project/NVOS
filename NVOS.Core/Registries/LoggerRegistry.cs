using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Registries
{
    public class LoggerRegistry : ICoreComponentRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<BufferingLogger>().AsSelf().As<ILogger>().SingleInstance();
        }
    }
}
