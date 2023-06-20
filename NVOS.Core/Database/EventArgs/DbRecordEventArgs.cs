using System;

namespace NVOS.Core.Database.EventArgs
{
    public class DbRecordEventArgs : System.EventArgs
    {
        public string CollectionName;
        public string Key;

        public DbRecordEventArgs(string collectionName, string key)
        {
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }
    }
}
