using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NVOS.Core.Database
{
    public class BinaryDbSerializer : IDbSerializer
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
