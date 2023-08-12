using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.gRPC;
using NVOS.Network.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Services
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(RpcConnectionService))]
    public class LedControllerService : IService
    {
        private RpcConnectionService rpcConnectionService;

        private GrpcChannel channel;
        private LEDController.LEDControllerClient client;

        public bool Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<RpcConnectionService>();

            channel = rpcConnectionService.GetChannel();
            if (channel != null)
                client = new LEDController.LEDControllerClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
            return true;
        }

        private void RpcConnectionService_ChannelConnected(object sender, EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new LEDController.LEDControllerClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, EventArgs e)
        {
            channel = null;
            client = null;
        }

        public DeviceState GetState(Guid address)
        {
            AssertClient();

            GetStateRequest request = new GetStateRequest();
            request.Address = address.ToString();
            GetStateResponse response = client.GetState(request);

            DeviceState state = new DeviceState(response.PoweredOn, response.Brightness, response.Mode);
            return state;
        }

        public async Task<DeviceState> GetStateAsync(Guid address)
        {
            AssertClient();

            GetStateRequest request = new GetStateRequest();
            request.Address = address.ToString();
            GetStateResponse response = await client.GetStateAsync(request);

            DeviceState state = new DeviceState(response.PoweredOn, response.Brightness, response.Mode);
            return state;
        }

        public void SetBrightness(Guid address, float brightness)
        {
            AssertClient();

            SetBrightnessRequest request = new SetBrightnessRequest();
            request.Address = address.ToString();
            request.Brightness = brightness;

            client.SetBrightness(request);
        }

        public async Task SetBrightnessAsync(Guid address, float brightness)
        {
            AssertClient();

            SetBrightnessRequest request = new SetBrightnessRequest();
            request.Address = address.ToString();
            request.Brightness = brightness;

            await client.SetBrightnessAsync(request);
        }

        public void SetMode(Guid address, LEDMode mode)
        {
            AssertClient();

            SetModeRequest request = new SetModeRequest();
            request.Address = address.ToString();
            request.Mode = mode;

            client.SetMode(request);
        }

        public async Task SetModeAsync(Guid address, LEDMode mode)
        {
            AssertClient();

            SetModeRequest request = new SetModeRequest();
            request.Address = address.ToString();
            request.Mode = mode;

            await client.SetModeAsync(request);
        }

        public void SetPowerState(Guid address, bool poweredOn)
        {
            AssertClient();

            SetPowerStateRequest request = new SetPowerStateRequest();
            request.Address = address.ToString();
            request.PoweredOn = poweredOn;

            client.SetPowerState(request);
        }

        public async Task SetPowerStateAsync(Guid address, bool poweredOn)
        {
            AssertClient();

            SetPowerStateRequest request = new SetPowerStateRequest();
            request.Address = address.ToString();
            request.PoweredOn = poweredOn;

            await client.SetPowerStateAsync(request);
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("LedControllerService is not connected to the server!");
        }
    }
}
