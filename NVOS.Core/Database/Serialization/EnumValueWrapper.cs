using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database.Serialization
{
    public struct EnumValueWrapper
    {
        public Type EnumType;
        public string Value;

        public EnumValueWrapper(Type enumType, string value)
        {
            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public EnumValueWrapper(object enumValue)
        {
            EnumType = enumValue.GetType();
            if (!EnumType.IsEnum)
                throw new ArgumentException("Object is not an enum");

            Value = enumValue.ToString();
        }

        public object GetInstance()
        {
            return Enum.Parse(EnumType, Value);
        }
    }
}
