using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database.Serialization
{
    public struct PrimitiveValueWrapper
    {
        public Type Type;
        public object Value;

        public PrimitiveValueWrapper(Type type, string serializedValue)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Value = serializedValue ?? throw new ArgumentNullException(nameof(serializedValue));
        }

        public PrimitiveValueWrapper(object obj)
        {
            Type = obj.GetType();
            if (!Type.IsPrimitive && !Type.IsEnum)
                throw new ArgumentException("Object is not a primitive type");

            if (Type.IsEnum)
                Value = obj.ToString();
            else
                Value = obj;
        }

        public object GetInstance()
        {
            if (Type.IsEnum)
                return Enum.Parse(Type, (string)Value);

            return Convert.ChangeType(Value, Type);
        }
    }
}
