using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public interface IDbAdapter
    {
        Guid CreateCollection(string name);
        bool DeleteCollection(Guid id);
        bool DeleteCollection(string name);
        DbCollectionInfo GetCollection(Guid id);
        DbCollectionInfo GetCollection(string name);
        IEnumerable<DbCollectionInfo> ListCollections();
        bool CollectionExists(Guid id);
        bool CollectionExists(string name);
        uint CountCollections();
        uint CountRecords(Guid collectionId);
        uint CountRecords(string collectionName);
        IEnumerable<KeyValuePair<string, object>> ListRecords(Guid collectionId);
        IEnumerable<KeyValuePair<string, object>> ListRecords(string collectionName);
        object ReadRecord(Guid collectionId, string key);
        object ReadRecord(string collectionName, string key);
        bool WriteRecord(Guid collectionId, string key, object value);
        bool WriteRecord(string collectionName, string key, object value);
    }
}
