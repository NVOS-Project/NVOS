using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
