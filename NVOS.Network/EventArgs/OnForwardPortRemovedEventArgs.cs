using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.EventArgs
{
    public class OnForwardPortRemovedEventArgs : System.EventArgs
    {
        public ushort ServerPort;

        public OnForwardPortRemovedEventArgs(ushort serverPort)
        {
            ServerPort = serverPort;
        }
    }
}
