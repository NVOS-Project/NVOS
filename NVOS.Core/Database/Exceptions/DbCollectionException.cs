using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database.Exceptions
{
    public class DbCollectionException : Exception
    {
        public string CollectionName;
        public Guid? CollectionId;
        public DbCollectionException(string message, string collectionName) : base(message)
        {
            CollectionName = collectionName;
            CollectionId = null;
        }

        public DbCollectionException(string message, Guid collectionId) : base(message)
        {
            CollectionName = null;
            CollectionId = collectionId;
        }
    }
}
