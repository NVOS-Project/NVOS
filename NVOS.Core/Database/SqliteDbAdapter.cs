using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

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

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Collections VALUES (@ID, @Name)";
                command.Parameters.AddWithValue("@ID", collectionId.ToString());
                command.Parameters.AddWithValue("@Name", name);
                command.ExecuteNonQuery();
            }

            return collectionId;
        }

        public void DeleteCollection(Guid id)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Collections WHERE ID='@ID'";
                command.Parameters.AddWithValue("@ID", id.ToString());
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Records WHERE CollectionID='@ID'";
                command.Parameters.AddWithValue("@ID", id.ToString());
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCollection(string name)
        {
            DbCollectionInfo info = GetCollection(name);

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Records WHERE CollectionID='@ID'";
                command.Parameters.AddWithValue("@ID", info.Id);
                command.ExecuteNonQuery();
            }

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Collections WHERE ID='@ID'";
                command.Parameters.AddWithValue("@ID", info.Id);
                command.ExecuteNonQuery();
            }
        }

        public DbCollectionInfo GetCollection(Guid id)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Name FROM Collections WHERE ID=@ID";
                command.Parameters.AddWithValue("@ID", id);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                string name = (string)reader["Name"];

                return new DbCollectionInfo(id, name);
            }
        }

        public DbCollectionInfo GetCollection(string name)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID FROM Collections WHERE Name=@Name";
                command.Parameters.AddWithValue("@Name", name);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                Guid id = Guid.Parse((string)reader["ID"]);

                return new DbCollectionInfo(id, name);
            }
        }

        public IEnumerable<DbCollectionInfo> ListCollections()
        {
            List<DbCollectionInfo> collectionList = new List<DbCollectionInfo>();
            
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID, Name FROM Collections";
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Guid id = Guid.Parse((string)reader["ID"]);
                    string name = (string)reader["Name"];

                    collectionList.Add(new DbCollectionInfo(id, name));
                }

                return collectionList;
            }
        }

        public bool CollectionExists(Guid id)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID, Name FROM Collections WHERE ID=@ID";
                command.Parameters.AddWithValue("@ID", id);
                SQLiteDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return false;
                }

                return true;
            }
        }

        public bool CollectionExists(string name)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID, Name FROM Collections WHERE Name=@Name";
                command.Parameters.AddWithValue("@Name", name);
                SQLiteDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return false;
                }

                return true;
            }
        }

        public uint CountCollections()
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(ID) FROM Collections";
                uint count = (uint)command.ExecuteScalar();

                return count;
            }
        }

        public uint CountRecords(Guid collectionId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(ID) FROM Records WHERE CollectionID=@ID";
                command.Parameters.AddWithValue("@ID", collectionId);
                uint count = (uint)command.ExecuteScalar();

                return count;
            }
        }

        public uint CountRecords(string collectionName)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(ID) FROM Records WHERE CollectionID=@ID";
                command.Parameters.AddWithValue("@ID", info.Id);
                uint count = (uint)command.ExecuteScalar();

                return count;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId)
        {
            List<KeyValuePair<string, object>> recordList = new List<KeyValuePair<string, object>>();

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Key, Value FROM Records WHERE CollectionID=@ID";
                command.Parameters.AddWithValue("@ID", collectionId);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string key = (string)reader["Key"];
                    object value = serializer.Deserialize((byte[])reader["Value"]);

                    recordList.Add(new KeyValuePair<string, object>(key, value));
                }

                return recordList;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName)
        {
            DbCollectionInfo info = GetCollection(collectionName);
            List<KeyValuePair<string, object>> recordList = new List<KeyValuePair<string, object>>();

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Key, Value FROM Records WHERE CollectionID=@ID";
                command.Parameters.AddWithValue("@ID", info.Id);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string key = (string)reader["Key"];
                    object value = serializer.Deserialize((byte[])reader["Value"]);

                    recordList.Add(new KeyValuePair<string, object>(key, value));
                }

                return recordList;
            }
        }

        public object ReadRecord(Guid collectionId, string key)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Value FROM Records WHERE CollectionID=@ID AND Key=@Key";
                command.Parameters.AddWithValue("@ID", collectionId);
                command.Parameters.AddWithValue("@Key", key);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                object value = serializer.Deserialize((byte[])reader["Value"]);

                return value;
            }
        }

        public object ReadRecord(string collectionName, string key)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Value FROM Records WHERE CollectionID=@ID AND Key=@Key";
                command.Parameters.AddWithValue("@ID", info.Id);
                command.Parameters.AddWithValue("@Key", key);
                SQLiteDataReader reader = command.ExecuteReader();

                reader.Read();
                object value = serializer.Deserialize((byte[])reader["Value"]);

                return value;
            }
        }

        public void WriteRecord(Guid collectionId, string key, object value)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                byte[] valueSerialized = serializer.Serialize(value);
                command.CommandText = "INSERT INTO Records (CollectionID, Key, Value) VALUES (@ID, @Key, @Value)";
                command.Parameters.AddWithValue("@ID", collectionId.ToString());
                command.Parameters.AddWithValue("@Key", key);
                command.Parameters.AddWithValue("@Value", valueSerialized);
                command.ExecuteNonQuery();
            }
        }

        public void WriteRecord(string collectionName, string key, object value)
        {
            DbCollectionInfo info = GetCollection(collectionName);

            using (SQLiteCommand command = connection.CreateCommand())
            {
                byte[] valueSerialized = serializer.Serialize(value);
                command.CommandText = "INSERT INTO Records (CollectionID, Key, Value) VALUES (@ID, @Key, @Value)";
                command.Parameters.AddWithValue("@ID", info.Id.ToString());
                command.Parameters.AddWithValue("@Key", key);
                command.Parameters.AddWithValue("@Value", valueSerialized);
                command.ExecuteNonQuery();
            }
        }

        public void Open(string file)
        {
            if (!File.Exists(file))
            {
                connection = new SQLiteConnection($"Data Source={file}");
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"
                        CREATE TABLE Collections (
                            ID TEXT NOT NULL UNIQUE,
                            Name  TEXT NOT NULL,
                            PRIMARY KEY(ID)
                        );

                        CREATE TABLE Records (
                            ID INTEGER NOT NULL UNIQUE,
                            CollectionID TEXT NOT NULL UNIQUE,
                            Key TEXT NOT NULL UNIQUE,
                            Value BLOB,
	                        FOREIGN KEY(CollectionID) REFERENCES Collections(ID),
	                        PRIMARY KEY(ID AUTOINCREMENT)
                        );
                    ";
                command.ExecuteNonQuery();
            }
            else
            {
                connection = new SQLiteConnection($"Data Source={file}");
                connection.Open();
            }
        }

        public void Close()
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("Connection is already closed!");
            }

            connection.Close();
        }
    }
}
