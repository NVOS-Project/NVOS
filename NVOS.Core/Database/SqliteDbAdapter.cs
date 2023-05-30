using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;

namespace NVOS.Core.Database
{
    public class SQLiteDbAdapter : IDbAdapter
    {
        public bool IsOpen { get; }

        private SQLiteConnection connection;
        private IDbSerializer serializer;

        public SQLiteDbAdapter(IDbSerializer serializer)
        {
            this.serializer = serializer;
        }

        public Guid CreateCollection(string name)
        {
            Guid collectionId = Guid.NewGuid();

            SQLiteCommand command = connection.CreateCommand("INSERT INTO Collections VALUES (@Id, @Name)");
            command.Bind("@Id", collectionId.ToString());
            command.Bind("@Name", name);
            command.ExecuteNonQuery();

            return collectionId;
        }

        public void DeleteCollection(Guid id)
        {
            SQLiteCommand commandCollections = connection.CreateCommand("DELETE FROM Collections WHERE Id='@Id'");
            commandCollections.Bind("@Id", id.ToString());
            commandCollections.ExecuteNonQuery();

            SQLiteCommand commandRecords = connection.CreateCommand("DELETE FROM Records WHERE CollectionId='@Id'");
            commandRecords.Bind("@Id", id.ToString());
            commandRecords.ExecuteNonQuery();
        }

        public void DeleteCollection(string name)
        {
            DbCollectionInfo info = GetCollection(name);
            DeleteCollection(info.Id);
        }

        public DbCollectionInfo GetCollection(Guid id)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT Id, Name FROM Collections WHERE Id=@Id");
            command.Bind("@Id", id);
            return command.ExecuteQuery<DbCollectionInfo>().FirstOrDefault();
        }

        public DbCollectionInfo GetCollection(string name)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT Id, Name FROM Collections WHERE Name=@Name");
            command.Bind("@Name", name);
            return command.ExecuteQuery<DbCollectionInfo>().FirstOrDefault();
        }

        public IEnumerable<DbCollectionInfo> ListCollections()
        {
            SQLiteCommand command = connection.CreateCommand("SELECT Id, Name FROM Collections");
            return command.ExecuteQuery<DbCollectionInfo>();
        }

        public bool CollectionExists(Guid id)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT COUNT(1) FROM Collections WHERE Id=@Id");
            command.Bind("@Id", id);
            return command.ExecuteScalar<uint>() != 0;
        }

        public bool CollectionExists(string name)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT COUNT(1) FROM Collections WHERE Name=@Name");
            command.Bind("@Name", name);
            return command.ExecuteScalar<uint>() != 0;
        }

        public uint CountCollections()
        {
            SQLiteCommand command = connection.CreateCommand("SELECT COUNT(*) FROM Collections");
            return command.ExecuteScalar<uint>();
        }

        public uint CountRecords(Guid collectionId)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT COUNT(*) FROM Records WHERE CollectionId=@Id");
            command.Bind("@Id", collectionId);
            return command.ExecuteScalar<uint>();
        }

        public uint CountRecords(string collectionName)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            return CountRecords(info.Id);
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT * FROM Records WHERE CollectionId=@Id");
            command.Bind("@Id", collectionId);
            List<Record> recordList = command.ExecuteQuery<Record>();

            foreach (Record record in recordList)
            {
                yield return new KeyValuePair<string, object>(record.Key, serializer.Deserialize(record.Value));
            }
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            return ListRecords(info.Id);
        }

        public object ReadRecord(Guid collectionId, string key)
        {
            SQLiteCommand command = connection.CreateCommand("SELECT * FROM Records WHERE CollectionId=@Id AND Key=@Key");
            command.Bind("@Id", collectionId);
            command.Bind("@Key", key);

            Record record = command.ExecuteQuery<Record>().FirstOrDefault();
            return serializer.Deserialize(record.Value);
        }

        public object ReadRecord(string collectionName, string key)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            return ReadRecord(info.Id, key);
        }

        public void WriteRecord(Guid collectionId, string key, object value)
        {
            SQLiteCommand selectCommand = connection.CreateCommand("SELECT * FROM Records WHERE CollectionId=@Id AND Key=@Key");
            selectCommand.Bind("@Id", collectionId);
            selectCommand.Bind("@Key", key);

            Record record = selectCommand.ExecuteQuery<Record>().FirstOrDefault();
            string valueSerialized = serializer.Serialize(value);

            if (record == null)
            {
                Record newRecord = new Record(collectionId, key, valueSerialized);
                connection.Insert(newRecord);

                return;
            }

            record.Value = valueSerialized;
            connection.Update(record);
        }

        public void WriteRecord(string collectionName, string key, object value)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            WriteRecord(info.Id, key, value);
        }

        public void Open(string file)
        {
            connection = new SQLiteConnection(file);
            connection.CreateTable<Collection>();
            connection.CreateTable<Record>();
            connection.CreateIndex("UX_Records_CollectionId_Key", "Records", new[] { "CollectionId", "Key" }, true);
        }

        public void Close()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Connection is already closed!");
            }

            connection.Close();
            connection = null;
        }
    }
}
