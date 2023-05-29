using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public class BinarySerializer : IDbSerializer
    {
        public byte[] Serialize(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public object Deserialize(byte[] obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(obj))
            {
                return formatter.Deserialize(ms);
            }
        }
    }
}
