using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.gRPC;
using Port = NVOS.Network.Structs.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Services
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(RpcConnectionService))]
    public class NetworkManagerService : IService
    {
        private RpcConnectionService rpcConnectionService;

        private GrpcChannel channel;
        private NetworkManager.NetworkManagerClient client;

        public bool Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<RpcConnectionService>();
            channel = rpcConnectionService.GetChannel();
            client = new NetworkManager.NetworkManagerClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
            return true;
        }

        private void RpcConnectionService_ChannelConnected(object sender, EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new NetworkManager.NetworkManagerClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, EventArgs e)
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
        }

        public async Task AddForwardPortAsync(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            await client.AddForwardPortAsync(request);
        }

        public void AddReversePort(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            client.AddReversePort(request);
        }

        public async Task AddReversePortAsync(ushort serverPort, ushort devicePort)
        {
            AssertClient();

            AddPortRequest request = new AddPortRequest();
            request.ServerPort = serverPort;
            request.DevicePort = devicePort;

            await client.AddReversePortAsync(request);
        }

        public void RemoveForwardPort(ushort serverPort)
        {
            AssertClient();

            RemoveForwardPortRequest request = new RemoveForwardPortRequest();
            request.ServerPort = serverPort;

            client.RemoveForwardPort(request);
        }

        public async Task RemoveForwardPortAsync(ushort serverPort)
        {
            AssertClient();

            RemoveForwardPortRequest request = new RemoveForwardPortRequest();
            request.ServerPort = serverPort;

            await client.RemoveForwardPortAsync(request);
        }

        public void RemoveReversePort(ushort devicePort)
        {
            AssertClient();

            RemoveReversePortRequest request = new RemoveReversePortRequest();
            request.DevicePort = devicePort;

            client.RemoveReversePort(request);
        }

        public async Task RemoveReversePortAsync(ushort devicePort)
        {
            AssertClient();

            RemoveReversePortRequest request = new RemoveReversePortRequest();
            request.DevicePort = devicePort;

            await client.RemoveReversePortAsync(request);
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("NetworkManagerService is not connected to the server!");
        }
    }
}
