using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Structs
{
    public class Port
    {
        public PortType Type;
        public ushort LocalPort;
        public ushort RemotePort;

        public Port(PortType type, ushort localPort, ushort remotePort)
        {
            Type = type;
            LocalPort = localPort;
            RemotePort = remotePort;
        }
    }
}
