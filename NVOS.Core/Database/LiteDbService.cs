using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public class LiteDbService : IDatabaseService
    {
        private LiteDatabase db;

        public DbCollection this[string collectionName] => GetCollection(collectionName);

        public bool Init()
        {
            return true;
        }

        public void Open(string file)
        {
            db = new LiteDatabase(file);
        }

        public void Close()
        {
            db?.Dispose();
            db = null;
        }

        public void Dispose()
        {
            Close();
        }

        private void CreateCollection(string collectionName)
        {
            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            collection.EnsureIndex("keyIndex", x => x.Key, unique: true);
        }

        public DbCollection GetCollection(string collectionName)
        {
            return new DbCollection(collectionName, this);
        }

        public bool DropCollection(string collectionName)
        {
            return db.DropCollection(collectionName);
        }

        public IEnumerable<string> ListCollections()
        {
            return db.GetCollectionNames();
        }

        public bool CollectionExists(string collectionName)
        {
            return db.CollectionExists(collectionName);
        }

        public int CountCollections()
        {
            return db.GetCollectionNames().Count();
        }

        public int CountRecords(string collectionName)
        {
            if (!CollectionExists(collectionName))
                return 0;

            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            return collection.Count();
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName)
        {
            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            IBsonDataReader reader = collection.Query().ExecuteReader();
            while(reader.Read())
            {
                yield return new KeyValuePair<string, object>(reader["Key"], reader["Value"]);
            }
        }

        public object Read(string collectionName, string key)
        {
            if (!CollectionExists(collectionName))
                throw new KeyNotFoundException(key);

            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            LiteDbRecord record = collection.Query().Where(x => x.Key == key).FirstOrDefault();
            if (record == null)
                throw new KeyNotFoundException(key);

            return record.Value;
        }

        public object ReadOrDefault(string collectionName, string key, object defaultValue)
        {
            try
            {
                object value = Read(collectionName, key);
                return value;
            }
            catch(KeyNotFoundException)
            {
                Write(collectionName, key, defaultValue);
                return defaultValue;
            }
        }

        public bool Write(string collectionName, string key, object value)
        {
            if (!CollectionExists(collectionName))
                CreateCollection(collectionName);

            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            LiteDbRecord record = collection.Query().Where(x => x.Key == key).FirstOrDefault();
            if (record == null)
                record = new LiteDbRecord(key, value);
            else
                record.Value = value;

            return collection.Upsert(record);
        }
    }
}