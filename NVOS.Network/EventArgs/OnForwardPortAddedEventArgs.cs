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
