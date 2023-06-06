using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
