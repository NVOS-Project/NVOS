using SQLite;
using System;

namespace NVOS.Core.Database
{
    [Table("Records")]
    public class Record
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        [Column("Id")]
        public int Id { get; set; }

        [NotNull]
        [Column("CollectionId")]
        public Guid CollectionId { get; set; }

        [NotNull]
        [Column("Key")]
        public string Key { get; set; }

        [Column("Value")]
        public byte[] Value { get; set; }

        public Record() { }

        public Record(Guid collectionId, string key, byte[] value)
        {
            CollectionId = collectionId;
            Key = key;
            Value = value;
        }
    }
}
