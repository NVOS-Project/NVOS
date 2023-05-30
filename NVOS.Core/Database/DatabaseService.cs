using System;
using System.Collections.Generic;


namespace NVOS.Core.Database
{
    public class DatabaseService
    {
        private IDbAdapter adapter;

        public DatabaseService(IDbAdapter adapter)
        {
            this.adapter = adapter;
        }

        public DbCollection this[string name]
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
                throw new InvalidOperationException("Collection already exists!");
            }


            return adapter.CreateCollection(name);
        }

        public void DeleteCollection(Guid id)
        {
            if (!CollectionExists(id))
            {
                throw new InvalidOperationException("Collection does not exist!");
            }

            adapter.DeleteCollection(id);
        }

        public void DeleteCollection(string name)
        {
            if (!CollectionExists(name))
            {
                throw new InvalidOperationException("Collection does not exist!");
            }

            adapter.DeleteCollection(name);
        }

        public IEnumerable<DbCollection> ListCollections()
        {
            foreach (DbCollectionInfo info in adapter.ListCollections())
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
                throw new InvalidOperationException("Collection does not exist!");
            }

            return adapter.CountRecords(collectionId);
        }

        public uint CountRecords(string collectionName)
        {
            if (!CollectionExists(collectionName))
            {
                throw new InvalidOperationException("Collection does not exist!");
            }

            return adapter.CountRecords(collectionName);
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId)
        {
            if (!CollectionExists(collectionId))
            {
                throw new InvalidOperationException("Collection does not exist!");
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
                throw new InvalidOperationException("Collection does not exist!");
            }

            foreach (KeyValuePair<string, object> record in adapter.ListRecords(collectionName))
            {
                yield return record;
            }
        }

        public object Read(Guid collectionId, string key)
        {
            return adapter.ReadRecord(collectionId, key);
        }

        public object Read(string collectionName, string key)
        {
            return adapter.ReadRecord(collectionName, key);
        }

        public void Write(Guid collectionId, string key, object value)
        {
            adapter.WriteRecord(collectionId, key, value);
        }

        public void Write(string collectionName, string key, object value)
        {
            adapter.WriteRecord(collectionName, key, value);
        }

        public DbCollection GetCollection(Guid id)
        {
            if (!CollectionExists(id))
            {
                throw new InvalidOperationException("Collection does not exist!");
            }

            DbCollectionInfo info = adapter.GetCollection(id);
            return new DbCollection(info, this);
        }

        public DbCollection GetCollection(string name)
        {
            if (!CollectionExists(name))
            {
                throw new InvalidOperationException("Collection does not exist!");
            }

            DbCollectionInfo info = adapter.GetCollection(name);
            return new DbCollection(info, this);
        }
    }
}
