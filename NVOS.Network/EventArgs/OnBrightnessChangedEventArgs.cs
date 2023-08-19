using System;

namespace NVOS.Network.EventArgs
{
    public class OnBrightnessChangedEventArgs : System.EventArgs
    {
        public Guid Address;
        public float Brightness;

        public OnBrightnessChangedEventArgs(Guid address, float brightness)
        {
            Address = address;
            Brightness = brightness;
        }
    }
}
