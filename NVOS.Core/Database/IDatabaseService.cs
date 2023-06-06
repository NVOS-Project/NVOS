using NVOS.Core.Database.EventArgs;
using NVOS.Core.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NVOS.Core.Database
{
    public interface IDatabaseService : IDisposable
    {
        event EventHandler<DbCollectionEventArgs> CollectionCreated;
        event EventHandler<DbCollectionEventArgs> CollectionDropped;
        event EventHandler<DbRecordEventArgs> RecordRead;
        event EventHandler<DbRecordEventArgs> RecordWritten;
        DbCollection this[string collectionName] { get; }
        DbCollection GetCollection(string collectionName);
        bool DropCollection(string collectionName);
        IEnumerable<string> ListCollections();
        bool CollectionExists(string collectionName);
        int CountCollections();
        int CountRecords(string collectionName);
        IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName);
        object Read(string collectionName, string key);
        bool Write(string collectionName, string key, object value);
    }
}
