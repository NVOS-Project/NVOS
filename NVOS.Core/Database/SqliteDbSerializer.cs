using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace NVOS.Core.Database
{
    public class SQLiteDbSerializer : IDbSerializer
    {
        public byte[] Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);

                return ms.ToArray();
            }
        }

        public object Deserialize(byte[] obj)
        {
            MemoryStream ms = new MemoryStream(obj);
            using (BsonDataReader reader = new BsonDataReader(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                object deserializedObject = serializer.Deserialize<object>(reader);

                return deserializedObject;
            }
        }
    }
}
