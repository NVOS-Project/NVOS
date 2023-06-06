using NVOS.Core.Database.EventArgs;
using System;
using System.Collections.Generic;

namespace NVOS.Core.Database
{
    public class DbCollection : IDisposable
    {
        private bool isDisposed;
        private IDatabaseService databaseService;
        public string Name { get; private set; }
        public EventHandler<DbRecordEventArgs> RecordRead;
        public EventHandler<DbRecordEventArgs> RecordWritten;

        public DbCollection(string name, IDatabaseService service)
        {
            Name = name;
            databaseService = service;
            databaseService.RecordRead += DatabaseService_RecordRead;
            databaseService.RecordWritten += DatabaseService_RecordWritten;
        }

        private void DatabaseService_RecordRead(object sender, DbRecordEventArgs e)
        {
            if (e.CollectionName == Name)
                RecordRead?.Invoke(this, e);
        }

        private void DatabaseService_RecordWritten(object sender, DbRecordEventArgs e)
        {
            if (e.CollectionName == Name)
                RecordWritten?.Invoke(this, e);
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            databaseService.RecordRead -= DatabaseService_RecordRead;
            databaseService.RecordWritten -= DatabaseService_RecordWritten;
            isDisposed = true;
        }

        public object Read(string key)
        {
            return databaseService.Read(Name, key);
        }

        public object ReadOrDefault(string key, object defaultValue)
        {
            return databaseService.ReadOrDefault(Name, key, defaultValue);
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
