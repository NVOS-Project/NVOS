using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
