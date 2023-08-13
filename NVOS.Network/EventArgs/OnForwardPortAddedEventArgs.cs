﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.EventArgs
{
    public class OnForwardPortAddedEventArgs : System.EventArgs
    {
        public ushort ServerPort;
        public ushort DevicePort;

        public OnForwardPortAddedEventArgs(ushort serverPort, ushort devicePort)
        {
            ServerPort = serverPort;
            DevicePort = devicePort;
        }
    }
}
