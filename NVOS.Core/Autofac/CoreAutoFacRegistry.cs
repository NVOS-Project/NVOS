using Autofac;
using NVOS.Core.Database;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Autofac
{
    internal class CoreAutoFacRegistry
    {
        private IDatabaseService databaseService;

        public CoreAutoFacRegistry(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public void RegisterComponents(ContainerBuilder builder)
        {

            IEnumerable<Type> coreServices = Assembly.GetCallingAssembly()
                .GetTypes().Where(x => typeof(ICoreService).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract);

            foreach (Type coreServiceType in coreServices)
            {
                builder.RegisterType(coreServiceType).InstancePerLifetimeScope();
            }

            builder.RegisterInstance(databaseService).As<IDatabaseService>().ExternallyOwned();
        }
    }
}
