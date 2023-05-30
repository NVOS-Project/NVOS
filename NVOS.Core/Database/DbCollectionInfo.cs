using System;

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
