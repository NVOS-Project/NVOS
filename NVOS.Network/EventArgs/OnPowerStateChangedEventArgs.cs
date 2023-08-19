using System;

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
