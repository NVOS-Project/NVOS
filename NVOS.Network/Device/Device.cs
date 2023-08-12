using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Device
{
    public class Device
    {
        public Guid Address { get; set; }
        public List<int> Capabilities { get; set; }

        public Device(Guid address, List<int> capabilities) 
        {
            Address = address;
            Capabilities = capabilities;
        }
    }
}
