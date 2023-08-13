using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Logger;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.EventArgs;
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

        public event EventHandler<OnPowerStateChangedEventArgs> OnPowerStateChanged;
        public event EventHandler<OnBrightnessChangedEventArgs> OnBrightnessChanged;
        public event EventHandler<OnModeChangedEventArgs> OnModeChanged;

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

        private void RpcConnectionService_ChannelConnected(object sender, System.EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new LEDController.LEDControllerClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, System.EventArgs e)
        {
            channel = null;
            client = null;
        }

        public LEDState GetState(Guid address)
        {
            AssertClient();

            GetStateRequest request = new GetStateRequest();
            request.Address = address.ToString();
            GetStateResponse response = client.GetState(request);

            LEDState state = new LEDState(response.PoweredOn, response.Brightness, response.Mode);
            return state;
        }

        public async Task<LEDState> GetStateAsync(Guid address)
        {
            AssertClient();

            GetStateRequest request = new GetStateRequest();
            request.Address = address.ToString();
            GetStateResponse response = await client.GetStateAsync(request);

            LEDState state = new LEDState(response.PoweredOn, response.Brightness, response.Mode);
            return state;
        }

        public void SetBrightness(Guid address, float brightness)
        {
            AssertClient();

            SetBrightnessRequest request = new SetBrightnessRequest();
            request.Address = address.ToString();
            request.Brightness = brightness;

            client.SetBrightness(request);
            OnBrightnessChanged?.Invoke(this, new OnBrightnessChangedEventArgs(address, brightness));
        }

        public async Task SetBrightnessAsync(Guid address, float brightness)
        {
            AssertClient();

            SetBrightnessRequest request = new SetBrightnessRequest();
            request.Address = address.ToString();
            request.Brightness = brightness;

            await client.SetBrightnessAsync(request);
            OnBrightnessChanged?.Invoke(this, new OnBrightnessChangedEventArgs(address, brightness));
        }

        public void SetMode(Guid address, LEDMode mode)
        {
            AssertClient();

            SetModeRequest request = new SetModeRequest();
            request.Address = address.ToString();
            request.Mode = mode;

            client.SetMode(request);
            OnModeChanged?.Invoke(this, new OnModeChangedEventArgs(address, mode));
        }

        public async Task SetModeAsync(Guid address, LEDMode mode)
        {
            AssertClient();

            SetModeRequest request = new SetModeRequest();
            request.Address = address.ToString();
            request.Mode = mode;

            await client.SetModeAsync(request);
            OnModeChanged?.Invoke(this, new OnModeChangedEventArgs(address, mode));
        }

        public void SetPowerState(Guid address, bool poweredOn)
        {
            AssertClient();

            SetPowerStateRequest request = new SetPowerStateRequest();
            request.Address = address.ToString();
            request.PoweredOn = poweredOn;

            client.SetPowerState(request);
            OnPowerStateChanged?.Invoke(this, new OnPowerStateChangedEventArgs(address, poweredOn));
        }

        public async Task SetPowerStateAsync(Guid address, bool poweredOn)
        {
            AssertClient();

            SetPowerStateRequest request = new SetPowerStateRequest();
            request.Address = address.ToString();
            request.PoweredOn = poweredOn;

            await client.SetPowerStateAsync(request);
            OnPowerStateChanged?.Invoke(this, new OnPowerStateChangedEventArgs(address, poweredOn));
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("LedControllerService is not connected to the server!");
        }
    }
}
