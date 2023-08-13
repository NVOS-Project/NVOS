using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
