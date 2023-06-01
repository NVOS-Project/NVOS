using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public class LiteDbRecord
    {
        [BsonId(true)]
        public ObjectId Id { get; set; }
        [BsonField("Key")]
        public string Key { get; set; }
        [BsonField("Value")]
        public object Value { get; set; }

        public LiteDbRecord(string key, object value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value;
        }

        [BsonCtor]
        public LiteDbRecord()
        { }
    }
}
