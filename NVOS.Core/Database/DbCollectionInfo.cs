using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public struct DbCollectionInfo
    {
        public Guid Id;
        public string Name;

        public DbCollectionInfo(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
