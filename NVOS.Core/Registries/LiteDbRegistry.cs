using Autofac;
using NVOS.Core.Containers;
using NVOS.Core.Database;
using NVOS.Core.Database.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Registries
{
    public class LiteDbRegistry : ICoreComponentRegistry
    {
        private string dbPath;
        public LiteDbRegistry(string dbPath)
        {
            this.dbPath = dbPath;
        }

        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<LiteDbService>().WithParameter(new TypedParameter(typeof(string), dbPath)).AsSelf().As<IDatabaseService>().SingleInstance();
            builder.RegisterType<JsonDbValueSerializer>().AsSelf().As<IDbValueSerializer>().SingleInstance();
        }
    }
}
