using Newtonsoft.Json;
using System;

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
            if (deserialized.GetType() == typeof(PrimitiveValueWrapper))
            {
                // Unwrap the primitive
                return ((PrimitiveValueWrapper)deserialized).GetInstance();
            }

            return deserialized;
        }

        public string Serialize(object obj)
        {
            Type type = obj.GetType();
            if (type.IsPrimitive || type.IsEnum)
            {
                // Gotta wrap the primitive value
                obj = new PrimitiveValueWrapper(obj);
            }

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
