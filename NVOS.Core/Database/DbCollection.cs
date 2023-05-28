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
            throw new NotImplementedException();
        }

        public bool Write(string name, object value)
        {
            throw new NotImplementedException();
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public uint CountRecords()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords()
        {
            throw new NotImplementedException();
        }
    }
}
