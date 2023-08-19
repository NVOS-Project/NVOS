using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.EventArgs;
using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Port = NVOS.Network.Structs.Port;

namespace NVOS.Network.Services
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(RpcConnectionService))]
    public class NetworkManagerService : IService
    {
        private RpcConnectionService rpcConnectionService;

        private GrpcChannel channel;
        private NetworkManager.NetworkManagerClient client;

        public event EventHandler<OnForwardPortAddedEventArgs> OnForwardPortAdded;
        public event EventHandler<OnReversePortAddedEventArgs> OnReversePortAdded;
        public event EventHandler<OnForwardPortRemovedEventArgs> OnForwardPortRemoved;
        public event EventHandler<OnReversePortRemovedEventArgs> OnReversePortRemoved;

        public bool Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<RpcConnectionService>();

            channel = rpcConnectionService.GetChannel();
            if (channel != null)
                client = new NetworkManager.NetworkManagerClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
            return true;
        }

        private void RpcConnectionService_ChannelConnected(object sender, System.EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new NetworkManager.NetworkManagerClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, System.EventArgs e)
        {
            channel = null;
            client = null;
        }

        public IEnumerable<Port> GetRunningPorts()
        {
            AssertClient();

            GetRunningPortsResponse response = client.GetRunningPorts(new gRPC.Void());

            foreach (gRPC.Port port in response.Ports)
            {
                yield return new Port(port.Type, (ushort)port.LocalPort, (ushort)port.RemotePort);
            }
        }

        public async Task<List<Port>> GetRunningPortsAsync()
        {
            AssertClient();

            GetRunningPortsResponse response = await client.GetRunningPortsAsync(new gRPC.Void());

            List<Port> ports = new List<Port>();
            foreach (gRPC.Port port in response.Ports)
            {
                ports.Add(new Port(port.Type, (ushort)port.LocalPort, (ushort)port.RemotePort));
            }

            return ports;
        }

        public void AddForwardPort(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            client.AddForwardPort(request);
            OnForwardPortAdded?.Invoke(this, new OnForwardPortAddedEventArgs(serverPort, devicePort));
        }

        public async Task AddForwardPortAsync(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            await client.AddForwardPortAsync(request);
            OnForwardPortAdded?.Invoke(this, new OnForwardPortAddedEventArgs(serverPort, devicePort));
        }

        public void AddReversePort(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            client.AddReversePort(request);
            OnReversePortAdded?.Invoke(this, new OnReversePortAddedEventArgs(serverPort, devicePort));
        }

        public async Task AddReversePortAsync(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            await client.AddReversePortAsync(request);
            OnReversePortAdded?.Invoke(this, new OnReversePortAddedEventArgs(serverPort, devicePort));
        }

        public void RemoveForwardPort(ushort serverPort)
        {
            AssertClient();

            RemoveForwardPortRequest request = new RemoveForwardPortRequest();
            request.ServerPort = serverPort;

            client.RemoveForwardPort(request);
            OnForwardPortRemoved?.Invoke(this, new OnForwardPortRemovedEventArgs(serverPort));
        }

        public async Task RemoveForwardPortAsync(ushort serverPort)
        {
            AssertClient();

            RemoveForwardPortRequest request = new RemoveForwardPortRequest();
            request.ServerPort = serverPort;

            await client.RemoveForwardPortAsync(request);
            OnForwardPortRemoved?.Invoke(this, new OnForwardPortRemovedEventArgs(serverPort));
        }

        public void RemoveReversePort(ushort devicePort)
        {
            AssertClient();

            RemoveReversePortRequest request = new RemoveReversePortRequest();
            request.DevicePort = devicePort;

            client.RemoveReversePort(request);
            OnReversePortRemoved?.Invoke(this, new OnReversePortRemovedEventArgs(devicePort));
        }

        public async Task RemoveReversePortAsync(ushort devicePort)
        {
            AssertClient();

            RemoveReversePortRequest request = new RemoveReversePortRequest();
            request.DevicePort = devicePort;

            await client.RemoveReversePortAsync(request);
            OnReversePortRemoved?.Invoke(this, new OnReversePortRemovedEventArgs(devicePort));
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("NetworkManagerService is not connected to the server!");
        }
    }
}
