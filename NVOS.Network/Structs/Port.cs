using NVOS.Network.gRPC;

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
