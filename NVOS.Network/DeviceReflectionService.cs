using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Device = NVOS.Network.Structs.Device;

namespace NVOS.Network
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(EmbeddedNetworkService))]
    public class DeviceReflectionService : IService
    {
        private EmbeddedNetworkService rpcConnectionService;

        private GrpcChannel channel;
        private DeviceReflection.DeviceReflectionClient client;

        public void Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<EmbeddedNetworkService>();

            channel = rpcConnectionService.GetChannel();
            if (channel != null)
                client = new DeviceReflection.DeviceReflectionClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
        }

        private void RpcConnectionService_ChannelConnected(object sender, System.EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new DeviceReflection.DeviceReflectionClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, System.EventArgs e)
        {
            channel = null;
            client = null;
        }

        public IEnumerable<Device> GetDevices()
        {
            AssertClient();

            ListDevicesResponse response = client.ListDevices(new gRPC.Void());

            foreach (gRPC.Device responseDevice in response.Devices)
            {
                yield return ConvertDevice(responseDevice);
            }
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            AssertClient();

            ListDevicesResponse response = await client.ListDevicesAsync(new gRPC.Void());

            List<Device> devices = new List<Device>();
            foreach (gRPC.Device responseDevice in response.Devices)
            {
                devices.Add(ConvertDevice(responseDevice));
            }

            return devices;
        }

        public Device GetDeviceByAddress(Guid address)
        {
            AssertClient();

            ListDevicesResponse response = client.ListDevices(new gRPC.Void());
            gRPC.Device responseDevice = response.Devices.FirstOrDefault(x => x.Address == address.ToString());

            if (responseDevice == null)
                return null;

            return ConvertDevice(responseDevice);
        }

        public async Task<Device> GetDeviceByAddressAsync(Guid address)
        {
            AssertClient();

            ListDevicesResponse response = await client.ListDevicesAsync(new gRPC.Void());
            gRPC.Device responseDevice = response.Devices.FirstOrDefault(x => x.Address == address.ToString());

            if (responseDevice == null)
                return null;

            return ConvertDevice(responseDevice);
        }

        public IEnumerable<Device> GetDevicesWithCapability(CapabilityId capabilityId)
        {
            AssertClient();

            ListDevicesResponse response = client.ListDevices(new gRPC.Void());

            foreach (gRPC.Device responseDevice in response.Devices.Where(x => x.Capabilities.Contains(capabilityId)))
            {
                yield return ConvertDevice(responseDevice);
            }
        }

        public async Task<List<Device>> GetDevicesWithCapabilityAsync(CapabilityId capabilityId)
        {
            AssertClient();

            ListDevicesResponse response = await client.ListDevicesAsync(new gRPC.Void());

            List<Device> devices = new List<Device>();
            foreach (gRPC.Device responseDevice in response.Devices.Where(x => x.Capabilities.Contains(capabilityId)))
            {
                devices.Add(ConvertDevice(responseDevice));
            }

            return devices;
        }

        public int GetDeviceCount()
        {
            AssertClient();

            ListDevicesResponse response = client.ListDevices(new gRPC.Void());

            return (int)response.Count;
        }

        public async Task<int> GetDeviceCountAsync()
        {
            AssertClient();

            ListDevicesResponse response = await client.ListDevicesAsync(new gRPC.Void());

            return (int)response.Count;
        }

        public IEnumerable<string> GetBusControllers()
        {
            AssertClient();

            ListControllersResponse response = client.ListControllers(new gRPC.Void());

            foreach (BusController controller in response.Controllers)
            {
                yield return controller.Name;
            }
        }

        public async Task<List<string>> GetBusControllersAsync()
        {
            AssertClient();

            ListControllersResponse response = await client.ListControllersAsync(new gRPC.Void());

            List<string> controllers = new List<string>();
            foreach (BusController controller in response.Controllers)
            {
                controllers.Add(controller.Name);
            }

            return controllers;
        }

        public int GetBusControllerCount()
        {
            AssertClient();

            ListControllersResponse response = client.ListControllers(new gRPC.Void());

            return (int)response.Count;
        }

        public async Task<int> GetBusControllerCountAsync()
        {
            AssertClient();

            ListControllersResponse response = await client.ListControllersAsync(new gRPC.Void());

            return (int)response.Count;
        }

        private Device ConvertDevice(gRPC.Device responseDevice)
        {
            List<CapabilityId> capabilities = new List<CapabilityId>();
            foreach (CapabilityId capability in responseDevice.Capabilities)
                capabilities.Add(capability);

            return new Device(Guid.Parse(responseDevice.Address), capabilities);
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("DeviceReflectionService is not connected to the server!");
        }
    }
}
