using System;

namespace NVOS.Core.Database.EventArgs
{
    public class DbCollectionEventArgs : System.EventArgs
    {
        public string Name;

        public DbCollectionEventArgs(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
