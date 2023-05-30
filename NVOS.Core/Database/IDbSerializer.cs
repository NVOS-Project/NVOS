using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public interface IDbSerializer
    {
        string Serialize(object obj);
        object Deserialize(string obj);
    }
}
