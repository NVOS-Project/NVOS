using LiteDB;
using NVOS.Core.Database.EventArgs;
using NVOS.Core.Database.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVOS.Core.Database
{
    public class LiteDbService : IDatabaseService
    {
        private LiteDatabase db;
        private IDbValueSerializer serializer;
        public event EventHandler<DbCollectionEventArgs> CollectionCreated;
        public event EventHandler<DbCollectionEventArgs> CollectionDropped;
        public event EventHandler<DbRecordEventArgs> RecordRead;
        public event EventHandler<DbRecordEventArgs> RecordWritten;

        public DbCollection this[string collectionName] => GetCollection(collectionName);

        public LiteDbService(IDbValueSerializer serializer, string dbPath)
        {
            this.serializer = serializer;
            db = new LiteDatabase(dbPath);
        }

        public void Dispose()
        {
            db?.Dispose();
            db = null;
        }

        private void CreateCollection(string collectionName)
        {
            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            collection.EnsureIndex("keyIndex", x => x.Key, unique: true);
            CollectionCreated?.Invoke(this, new DbCollectionEventArgs(collectionName));
        }

        public DbCollection GetCollection(string collectionName)
        {
            return new DbCollection(collectionName, this);
        }

        public bool DropCollection(string collectionName)
        {
            bool result = db.DropCollection(collectionName);
            if (result)
                CollectionDropped?.Invoke(this, new DbCollectionEventArgs(collectionName));

            return result;
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
            while (reader.Read())
            {
                yield return new KeyValuePair<string, object>(reader["Key"], reader["Value"]);
            }
        }

        public object Read(string collectionName, string key)
        {
            RecordRead?.Invoke(this, new DbRecordEventArgs(collectionName, key));

            if (!CollectionExists(collectionName))
                throw new KeyNotFoundException(key);

            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            LiteDbRecord record = collection.Query().Where(x => x.Key == key).FirstOrDefault();
            if (record == null)
                throw new KeyNotFoundException(key);

            object value = serializer.Deserialize(record.Value);
            return value;
        }

        public object ReadOrDefault(string collectionName, string key, object defaultValue)
        {
            try
            {
                object value = Read(collectionName, key);
                return value;
            }
            catch (KeyNotFoundException)
            {
                Write(collectionName, key, defaultValue);
                return defaultValue;
            }
        }

        public bool Write(string collectionName, string key, object value)
        {
            if (!CollectionExists(collectionName))
                CreateCollection(collectionName);

            string valueSerialized = serializer.Serialize(value);
            ILiteCollection<LiteDbRecord> collection = db.GetCollection<LiteDbRecord>(collectionName);
            LiteDbRecord record = collection.Query().Where(x => x.Key == key).FirstOrDefault();
            if (record == null)
                record = new LiteDbRecord(key, valueSerialized);
            else
                record.Value = valueSerialized;

            bool result = collection.Upsert(record);
            RecordWritten?.Invoke(this, new DbRecordEventArgs(collectionName, key));

            return true;
        }
    }
}