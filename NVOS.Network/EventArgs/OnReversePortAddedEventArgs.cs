namespace NVOS.Network.EventArgs
{
    public class OnReversePortAddedEventArgs : System.EventArgs
    {
        public ushort ServerPort;
        public ushort DevicePort;

        public OnReversePortAddedEventArgs(ushort serverPort, ushort devicePort)
        {
            ServerPort = serverPort;
            DevicePort = devicePort;
        }
    }
}
