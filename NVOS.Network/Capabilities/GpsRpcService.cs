using Grpc.Net.Client;
using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Network.gRPC;
using NVOS.Network.Structs;
using System;
using System.Threading.Tasks;

namespace NVOS.Network.Capabilities
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(RpcConnectionService))]
    public class GpsRpcService : IService
    {
        private RpcConnectionService rpcConnectionService;

        private GrpcChannel channel;
        private Gps.GpsClient client;

        public bool Init()
        {
            rpcConnectionService = ServiceLocator.Resolve<RpcConnectionService>();
            channel = rpcConnectionService.GetChannel();

            if (channel != null)
                client = new Gps.GpsClient(channel);

            rpcConnectionService.ChannelConnected += RpcConnectionService_ChannelConnected;
            rpcConnectionService.ChannelLost += RpcConnectionService_ChannelLost;
            return true;
        }

        private void RpcConnectionService_ChannelConnected(object sender, System.EventArgs e)
        {
            channel = rpcConnectionService.GetChannel();
            client = new Gps.GpsClient(channel);
        }

        private void RpcConnectionService_ChannelLost(object sender, System.EventArgs e)
        {
            channel = null;
            client = null;
        }

        public GpsLocation GetLocation(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetLocationResponse response = client.GetLocation(request);

            return new GpsLocation(response.Latitude, response.Longitude);
        }

        public async Task<GpsLocation> GetLocationAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetLocationResponse response = await client.GetLocationAsync(request);

            return new GpsLocation(response.Latitude, response.Longitude);
        }

        public float GetAltitude(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAltitudeResponse response = client.GetAltitude(request);

            return response.Altitude;
        }

        public async Task<float> GetAltitudeAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAltitudeResponse response = await client.GetAltitudeAsync(request);

            return response.Altitude;
        }

        public bool HasFix(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            HasFixResponse response = client.HasFix(request);

            return response.HasFix;
        }

        public async Task<bool> HasFixAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            HasFixResponse response = await client.HasFixAsync(request);

            return response.HasFix;
        }

        public float GetSpeedOverGround(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetSpeedResponse response = client.GetSpeed(request);

            return response.SpeedOverGround;
        }

        public async Task<float> GetSpeedOverGroundAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetSpeedResponse response = await client.GetSpeedAsync(request);

            return response.SpeedOverGround;
        }

        public float GetHeading(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetHeadingResponse response = client.GetHeading(request);

            return response.Heading;
        }

        public async Task<float> GetHeadingAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetHeadingResponse response = await client.GetHeadingAsync(request);

            return response.Heading;
        }

        public uint GetSatelliteCount(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetNumSatellitesResponse response = client.GetNumSatellites(request);

            return response.Count;
        }

        public async Task<uint> GetSatelliteCountAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetNumSatellitesResponse response = await client.GetNumSatellitesAsync(request);

            return response.Count;
        }

        public GpsReport GetFullReport(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetFullReportResponse response = client.GetFullReport(request);

            return new GpsReport(
                response.HasFix,
                response.Latitude,
                response.Longitude,
                response.Altitude,
                response.SpeedOverGround,
                response.Heading,
                response.SatelliteCount,
                response.VerticalAccuracy,
                response.HorizontalAccuracy
                );
        }

        public async Task<GpsReport> GetFullReportAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetFullReportResponse response = await client.GetFullReportAsync(request);

            return new GpsReport(
                response.HasFix,
                response.Latitude,
                response.Longitude,
                response.Altitude,
                response.SpeedOverGround,
                response.Heading,
                response.SatelliteCount,
                response.VerticalAccuracy,
                response.HorizontalAccuracy
                );
        }

        public float GetVerticalAccuracy(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAccuracyResponse response = client.GetVerticalAccuracy(request);

            return response.Accuracy;
        }

        public async Task<float> GetVerticalAccuracyAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAccuracyResponse response = await client.GetVerticalAccuracyAsync(request);

            return response.Accuracy;
        }

        public float GetHorizontalAccuracy(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAccuracyResponse response = client.GetVerticalAccuracy(request);

            return response.Accuracy;
        }

        public async Task<float> GetHorizontalAccuracyAsync(Guid address)
        {
            AssertClient();

            GpsRequest request = new GpsRequest();
            request.Address = address.ToString();
            GetAccuracyResponse response = await client.GetVerticalAccuracyAsync(request);

            return response.Accuracy;
        }

        private void AssertClient()
        {
            if (client == null)
                throw new InvalidOperationException("GpsService is not connected to the server!");
        }
    }
}
