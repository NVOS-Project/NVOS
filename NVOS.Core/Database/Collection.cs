using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    [Table("Collections")]
    public class Collection
    {
        [PrimaryKey, NotNull, Unique]
        [Column("Id")]
        public Guid Id { get; set; }

        [NotNull]
        [Column("Name")]
        public string Name { get; set; }

        public Collection() { }

        public Collection(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
