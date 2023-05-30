using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public class JsonSerializer : IDbSerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public object Deserialize(string obj)
        {
            return JsonConvert.DeserializeObject<object>(obj);
        }
    }
}
