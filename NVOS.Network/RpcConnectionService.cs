using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using NVOS.Core;
using NVOS.Core.Logger;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.Network.gRPC;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NVOS.Network
{
    [ServiceType(ServiceType.Singleton)]
    public class RpcConnectionService : IService, IDisposable
    {
        private const string HOSTNAME = "http://localhost";
        private const int PORT = 30000;
        private const int CONNECT_RETRY_ATTEMPTS = 3;

        private bool isConnected;
        private bool isDisposed;

        private GrpcChannel channel;
        private Heartbeat.HeartbeatClient heartbeatClient;
        private Mutex mutex;

        public event EventHandler ChannelConnected;
        public event EventHandler ChannelLost;

        private BufferingLogger logger;

        public bool Init()
        {
            mutex = new Mutex();
            isConnected = false;
            logger = ServiceLocator.Resolve<BufferingLogger>();

            Task.Run(() => RunWorker());

            return true;
        }

        public void Dispose()
        {
            mutex.WaitOne();
            isDisposed = true;
            mutex.ReleaseMutex();
        }

        private async Task RunWorker()
        {
            while (true)
            {
                mutex.WaitOne();
                bool isDisposed = this.isDisposed;
                mutex.ReleaseMutex();

                if (isDisposed)
                {
                    break;
                }


                try
                {
                    await DoHeartbeat();
                    await Task.Delay(3000);
                }
                catch (Exception ex)
                {
                    logger.Error($"RpcConnectionService worker crashed: {ex}");
                }
            }
        }

        private async Task DoHeartbeat()
        {
            mutex.WaitOne();

            GrpcChannel channel = this.channel;
            Heartbeat.HeartbeatClient heartbeatClient = this.heartbeatClient;

            mutex.ReleaseMutex();

            if (channel != null && heartbeatClient != null && isConnected)
            {
                try
                {
                    await heartbeatClient.PingAsync(new gRPC.Void());
                    return;
                }
                catch
                {
                    mutex.WaitOne();
                    isConnected = false;
                    mutex.ReleaseMutex();

                    logger.Debug("Lost RPC connection");
                    ChannelLost?.Invoke(this, System.EventArgs.Empty);
                }
            }

            try
            {
                GrpcWebHandler handler = new GrpcWebHandler(new HttpClientHandler());
                GrpcChannelOptions options = new GrpcChannelOptions { HttpHandler = handler };

                channel = GrpcChannel.ForAddress($"{HOSTNAME}:{PORT}", options);
                heartbeatClient = new Heartbeat.HeartbeatClient(channel);
            }
            catch (Exception ex)
            {
                logger.Debug($"Error while building RPC channel (possible issue): {ex}");
                return;
            }


            for (int i = 0; i < CONNECT_RETRY_ATTEMPTS; i++)
            {
                try
                {
                    await heartbeatClient.PingAsync(new gRPC.Void());
                }
                catch (Exception ex)
                {
                    logger.Debug($"Failed to ping heartbeat service: {ex}");
                    await Task.Delay(1000);
                    continue;
                }

                try
                {
                    mutex.WaitOne();
                    this.channel = channel;
                    this.heartbeatClient = heartbeatClient;
                    isConnected = true;
                    mutex.ReleaseMutex();

                    logger.Debug("Estabilished RPC connection");
                    ChannelConnected?.Invoke(this, System.EventArgs.Empty);
                    return;
                }
                catch (Exception ex)
                {
                    logger.Debug($"Error while raising ChannelConnected event: {ex}");
                    mutex.WaitOne();
                    isConnected = false;
                    mutex.ReleaseMutex();
                }
            }

            logger.Warn($"Attempted estabilishing a connection {CONNECT_RETRY_ATTEMPTS} times, backing off...");
        }

        public GrpcChannel GetChannel()
        {
            mutex.WaitOne();
            if (!isConnected)
            {
                mutex.ReleaseMutex();
                return null;
            }

            GrpcChannel channel = this.channel;
            mutex.ReleaseMutex();
            return channel;
        }
    }
}
