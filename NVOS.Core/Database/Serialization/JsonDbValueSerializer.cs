using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database.Serialization
{
    public class JsonDbValueSerializer : IDbValueSerializer
    {
        private JsonSerializerSettings settings;
        public JsonDbValueSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public JsonDbValueSerializer()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.None;
            settings.TypeNameHandling = TypeNameHandling.All;
            this.settings = settings;
        }

        public object Deserialize(string obj)
        {
            object deserialized = JsonConvert.DeserializeObject(obj, settings);
            if (deserialized.GetType() == typeof(EnumValueWrapper))
            {
                // Unwrap the enum
                return ((EnumValueWrapper)deserialized).GetInstance();
            }

            return deserialized;
        }

        public string Serialize(object obj)
        {
            Type type = obj.GetType();
            if (type.IsEnum)
            {
                // Gotta wrap the enum
                obj = new EnumValueWrapper(obj);
            }

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
