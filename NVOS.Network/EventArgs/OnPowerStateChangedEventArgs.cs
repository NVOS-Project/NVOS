using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.EventArgs
{
    public class OnPowerStateChangedEventArgs : System.EventArgs
    {
        public Guid Address;
        public bool PoweredOn;

        public OnPowerStateChangedEventArgs(Guid address, bool poweredOn)
        {
            Address = address;
            PoweredOn = poweredOn;
        }
    }
}
