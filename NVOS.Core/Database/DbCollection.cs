using System;
using System.Collections.Generic;

namespace NVOS.Core.Database
{
    public class DbCollection
    {
        private IDatabaseService databaseService;
        public string Name { get; private set; }

        public DbCollection(string name, IDatabaseService service)
        {
            Name = name;
            databaseService = service;
        }

        public object Read(string key)
        {
            return databaseService.Read(Name, key);
        }

        public void Write(string key, object value)
        {
            databaseService.Write(Name, key, value);
        }

        public object this[string key]
        {
            get { return Read(key); }
            set { Write(key, value); }
        }

        public int CountRecords()
        {
            return databaseService.CountRecords(Name);
        }

        public IEnumerable<KeyValuePair<string, object>> ListRecords()
        {
            return databaseService.ListRecords(Name);
        }
    }
}
