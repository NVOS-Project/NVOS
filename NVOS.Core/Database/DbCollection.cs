using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Core.Database
{
    public class DbCollection
    {
        private DbCollectionInfo dbCollectionInfo;
        private DatabaseService databaseService;

        public DbCollection(DbCollectionInfo info, DatabaseService service)
        {
            dbCollectionInfo = info;
            databaseService = service;
        }

        public object Read(string name)
        {
            return databaseService.Read(dbCollectionInfo.Id, name);
        }

        public void Write(string name, object value)
        {
            databaseService.Write(dbCollectionInfo.Id, name, value);
        }

        public object this[string name]
        {
            get { return Read(name); }
            set { Write(name, value); }
        }

        public uint CountRecords()
        {
            return databaseService.CountRecords(dbCollectionInfo.Id);
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords()
        {
            return databaseService.ListRecords(dbCollectionInfo.Id);
        }
    }
}
