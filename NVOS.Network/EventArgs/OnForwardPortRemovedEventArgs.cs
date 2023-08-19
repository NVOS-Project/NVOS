namespace NVOS.Network.EventArgs
{
    public class OnForwardPortRemovedEventArgs : System.EventArgs
    {
        public ushort ServerPort;

        public OnForwardPortRemovedEventArgs(ushort serverPort)
        {
            ServerPort = serverPort;
        }
    }
}
