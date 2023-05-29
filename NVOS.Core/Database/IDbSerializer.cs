using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public interface IDbSerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] obj);
    }
}
