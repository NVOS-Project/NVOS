using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;

namespace NVOS.Network.Structs
{
    public class Device
    {
        public Guid Address;
        public List<CapabilityId> Capabilities;

        public Device(Guid address, List<CapabilityId> capabilities)
        {
            Address = address;
            Capabilities = capabilities;
        }
    }
}
