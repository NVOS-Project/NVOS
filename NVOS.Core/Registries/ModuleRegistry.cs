using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Registries
{
    public class ModuleRegistry : ICoreComponentRegistry
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ModuleManager>().AsSelf().As<IModuleManager>().SingleInstance();
        }
    }
}
