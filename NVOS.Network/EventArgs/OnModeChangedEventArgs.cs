using NVOS.Network.gRPC;
using System;

namespace NVOS.Network.EventArgs
{
    public class OnModeChangedEventArgs : System.EventArgs
    {
        public Guid Address;
        public LEDMode LEDMode;

        public OnModeChangedEventArgs(Guid address, LEDMode ledMode)
        {
            Address = address;
            LEDMode = ledMode;
        }
    }
}
