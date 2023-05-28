using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NVOS.Core.Database
{
    public class SqliteDbAdapter : IDbAdapter
    {
        public bool IsOpen { get; }

        private SqliteConnection SqliteConnection;

        public SqliteDbAdapter(string dbFile)
        {
            Open(dbFile);
        }

        public Guid CreateCollection(string name)
        {
            Guid collectionId = Guid.NewGuid();

            using (SqliteCommand command = SqliteConnection.CreateCommand())
            {
                command.CommandText = $@"INSERT INTO Collections VALUES (@ID, @Name)";
                command.Parameters.AddWithValue("@ID", collectionId.ToString());
                command.Parameters.AddWithValue("@Name", name);
                command.ExecuteNonQuery();
            }

            return collectionId;
        }

        public bool DeleteCollection(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCollection(string name)
        {
            throw new NotImplementedException();
        }

        public DbCollectionInfo GetCollection(Guid id)
        {
            throw new NotImplementedException();
        }

        public DbCollectionInfo GetCollection(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DbCollectionInfo> ListCollections()
        {
            throw new NotImplementedException();
        }

        public bool CollectionExists(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool CollectionExists(string name)
        {
            throw new NotImplementedException();
        }

        public uint CountCollections()
        {
            throw new NotImplementedException();
        }

        public uint CountRecords(Guid collectionId)
        {
            throw new NotImplementedException();
        }

        public uint CountRecords(string collectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName)
        {
            throw new NotImplementedException();
        }

        public object ReadRecord(Guid collectionId, string key)
        {
            throw new NotImplementedException();
        }

        public object ReadRecord(string collectionName, string key)
        {
            throw new NotImplementedException();
        }

        public bool WriteRecord(Guid collectionId, string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool WriteRecord(string collectionName, string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Open(string file)
        {
            try
            {
                if (!File.Exists(file))
                {
                    SqliteConnection = new SqliteConnection($"Data Source={file}");
                    SqliteConnection.Open();
                    SqliteCommand command = SqliteConnection.CreateCommand();
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
                            Value TEXT,
	                        FOREIGN KEY(CollectionID) REFERENCES Collections(ID),
	                        PRIMARY KEY(ID AUTOINCREMENT)
                        );
                    ";
                    command.ExecuteNonQuery();
                }
                else
                {
                    SqliteConnection = new SqliteConnection($"Data Source={file}");
                    SqliteConnection.Open();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public bool Close()
        {
            if (!IsOpen)
            {
                return false;
            }

            SqliteConnection.Close();
            return true;
        }
    }
}
