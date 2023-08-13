using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.EventArgs
{
    public class OnReversePortRemovedEventArgs : System.EventArgs
    {
        public ushort DevicePort;

        public OnReversePortRemovedEventArgs(ushort devicePort)
        {
            DevicePort = devicePort;
        }
    }
}
