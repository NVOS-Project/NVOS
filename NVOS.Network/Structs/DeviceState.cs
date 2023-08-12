using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Structs
{
    public class DeviceState
    {
        public bool PoweredOn;
        public float Brightness;
        public LEDMode Mode;

        public DeviceState(bool poweredOn, float brightness, LEDMode mode)
        {
            PoweredOn = poweredOn;
            Brightness = brightness;
            Mode = mode;
        }
    }
}
