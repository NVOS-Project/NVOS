namespace NVOS.Network.EventArgs
{
    public class OnReversePortRemovedEventArgs : System.EventArgs
    {
        public ushort DevicePort;

        public OnReversePortRemovedEventArgs(ushort devicePort)
        {
            DevicePort = devicePort;
        }
    }
}
