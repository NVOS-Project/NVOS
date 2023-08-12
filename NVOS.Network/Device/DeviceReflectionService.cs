using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.Network.Device
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(RpcConnectionService))]
    public class DeviceReflectionService : IService
    {
        private RpcConnectionService rpcConnectionService;

        private GrpcChannel channel;
        private DeviceReflection.DeviceReflectionClient client;

        public bool Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<RpcConnectionService>();
            channel = rpcConnectionService.GetChannel();
            client = new DeviceReflection.DeviceReflectionClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
            return true;
        }

        private void RpcConnectionService_ChannelConnected(object sender, EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new DeviceReflection.DeviceReflectionClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, EventArgs e)
        {
            channel = null;
            client = null;
        }


    }
}
