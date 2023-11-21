using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;

namespace NVOS.Network.Structs
{
    public class Device
    {
        public Guid Address;
        public List<CapabilityId> Capabilities;
        string DeviceName;
        string DriverName;
        bool IsRunning;

        public Device(Guid address, List<CapabilityId> capabilities, string deviceName, string driverName, bool isRunning)
        {
            Address = address;
            Capabilities = capabilities;
            DeviceName = deviceName;
            DriverName = driverName;
            IsRunning = isRunning;
        }
    }
}
