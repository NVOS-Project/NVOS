using System;

namespace NVOS.Core.Modules
{
    public interface IModule
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        Version Version { get; }
    }
}
