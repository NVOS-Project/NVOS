using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database.Serialization
{
    public interface IDbValueSerializer
    {
        string Serialize(object obj);
        object Deserialize(string obj);
    }
}
