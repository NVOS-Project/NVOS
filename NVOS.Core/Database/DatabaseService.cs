using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NVOS.Core.Database.Exceptions;

namespace NVOS.Core.Database
{
    public class DatabaseService
    {
        private IDbAdapter adapter;
        private DbCollection this[string name]
        {
            get { return GetCollection(name); }
        }

        public Guid CreateCollection(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (CollectionExists(name))
            {
                throw new DbCollectionException("Collection already exists!", name);
            }

            
            return adapter.CreateCollection(name);
        }

        public bool DeleteCollection(Guid id)
        {
            if (!CollectionExists(id))
            {
                return false;
            }

            return adapter.DeleteCollection(id);
        }

        public bool DeleteCollection(string name)
        {
            if (!CollectionExists(name))
            {
                return false;
            }

            return adapter.DeleteCollection(name);
        }

        public IEnumerable<DbCollection> ListCollections()
        {
            foreach(DbCollectionInfo info in adapter.ListCollections())
            {
                yield return new DbCollection(info, this); ;
            }
        }

        public bool CollectionExists(Guid id)
        {
            return adapter.CollectionExists(id);
        }

        public bool CollectionExists(string name)
        {
            return adapter.CollectionExists(name);
        }

        public uint CountCollections()
        {
            return adapter.CountCollections();
        }

        public uint CountRecords(Guid collectionId)
        {
            if (!CollectionExists(collectionId))
            {
                throw new DbCollectionException("Collection does not exist!", collectionId);
            }

            return adapter.CountRecords(collectionId);
        }

        public uint CountRecords(string collectionName)
        {
            if (!CollectionExists(collectionName))
            {
                throw new DbCollectionException("Collection does not exist!", collectionName);
            }

            return adapter.CountRecords(collectionName);
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId)
        {
            if (!CollectionExists(collectionId))
            {
                throw new DbCollectionException("Collection does not exist!", collectionId);
            }

            foreach (KeyValuePair<string, object> record in adapter.ListRecords(collectionId))
            {
                yield return record;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName)
        {
            if (!CollectionExists(collectionName))
            {
                throw new DbCollectionException("Collection does not exist!", collectionName);
            }

            foreach (KeyValuePair<string, object> record in adapter.ListRecords(collectionName))
            {
                yield return record;
            }
        }

        public object Read(Guid collectionId, string key)
        {
            if (!CollectionExists(collectionId))
            {
                throw new DbCollectionException("Collection does not exist!", collectionId);
            }

            return adapter.ReadRecord(collectionId, key);
        }

        public object Read(string collectionName, string key)
        {
            if (!CollectionExists(collectionName))
            {
                throw new DbCollectionException("Collection does not exist!", collectionName);
            }

            return adapter.ReadRecord(collectionName, key);
        }

        public bool Write(string collectionName, string key, object value)
        {
            if (!CollectionExists(collectionName))
            {
                throw new DbCollectionException("Collection does not exist!", collectionName);
            }

            return adapter.WriteRecord(collectionName, key, value);
        }

        public DbCollection GetCollection(Guid id)
        {
            if (!CollectionExists(id))
            {
                throw new DbCollectionException("Collection does not exist!", id);
            }

            DbCollectionInfo info = adapter.GetCollection(id);
            return new DbCollection(info, this);
        }

        public DbCollection GetCollection(string name)
        {
            if (!CollectionExists(name))
            {
                throw new DbCollectionException("Collection does not exist!", name);
            }

            DbCollectionInfo info = adapter.GetCollection(name);
            return new DbCollection(info, this);
        }
    }
}
