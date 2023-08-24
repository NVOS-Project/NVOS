using NVOS.Core;
using NVOS.Network.Capabilities;
using NVOS.Network;
using NVOS.UI.Services;
using NVOS.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services;
using NVOS.UI.Models;
using UnityEngine;
using NVOS.UI.Models.EventArgs;
using NVOS.Network.Structs;
using System.Linq;

namespace NVOS.LED
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(QuickTileUIService))]
    [ServiceDependency(typeof(DeviceReflectionService))]
    [ServiceDependency(typeof(LedControllerRpcService))]
    [ServiceDependency(typeof(EmbeddedNetworkService))]
    [ServiceDependency(typeof(MainThreadExecutorService))]
    public class LedPowerTileService : IService, IDisposable
    {
        private bool isDisposed;
        private Guid? deviceAddress;
        private Core.Logger.ILogger logger;
        private QuickTileUIService quickTileUIService;
        private DeviceReflectionService deviceReflection;
        private LedControllerRpcService ledRpcService;
        private EmbeddedNetworkService networkService;
        private MainThreadExecutorService executorService;

        private SwitchTile powerTile;

        public void Init()
        {
            logger = ServiceLocator.Resolve<Core.Logger.ILogger>();
            quickTileUIService = ServiceLocator.Resolve<QuickTileUIService>();
            deviceReflection = ServiceLocator.Resolve<DeviceReflectionService>();
            ledRpcService = ServiceLocator.Resolve<LedControllerRpcService>();
            networkService = ServiceLocator.Resolve<EmbeddedNetworkService>();
            executorService = ServiceLocator.Resolve<MainThreadExecutorService>();

            SetupTile();

            if (networkService.IsConnected)
                OnRpcConnected();
            else
                powerTile.Enabled = false;

            networkService.ChannelConnected += NetworkService_ChannelConnected;
            networkService.ChannelLost += NetworkService_ChannelLost;

            ledRpcService.OnPowerStateChanged += LedRpcService_OnPowerStateChanged;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            OnRpcDisconnected();
            networkService.ChannelConnected -= NetworkService_ChannelConnected;
            networkService.ChannelLost -= NetworkService_ChannelLost;

            ledRpcService.OnPowerStateChanged -= LedRpcService_OnPowerStateChanged;
            powerTile.Dispose();

            quickTileUIService = null;
            deviceReflection = null;
            ledRpcService = null;
            networkService = null;
            executorService = null;
            isDisposed = true;
        }

        private void SetupTile()
        {
            powerTile = quickTileUIService.CreateSwitchTile("LED Power");
            powerTile.DeactivatedText = "LED Power: OFF";
            powerTile.ActivatedText = "LED Power: ON";
            powerTile.DeactivatedColor = new Color32(150, 30, 30, 255);
            powerTile.ActivatedColor = new Color32(30, 150, 50, 255);
            powerTile.OnValueChanged += LedPowerTile_OnValueChanged;
        }

        private void LedPowerTile_OnValueChanged(object sender, SwitchTileValueChangedEventArgs e)
        {
            try
            {
                if (deviceAddress == null)
                    return;

                executorService.ExecuteAsync(async () =>
                {
                    await ledRpcService.SetPowerStateAsync(deviceAddress.Value, e.Value);
                });

            }
            catch (Exception ex)
            {
                logger.Warn($"[LEDPowerTileService] Failed to set new power state: {ex}");
            }
        }

        private void OnRpcConnected()
        {
            Device device = deviceReflection.GetDevicesWithCapability(Network.gRPC.CapabilityId.Ledcontroller).FirstOrDefault();
            if (device != null)
            {
                LEDState state = ledRpcService.GetState(device.Address);

                executorService.Execute(() =>
                {
                    SetPowerTileValue(state.PoweredOn);

                    deviceAddress = device.Address;
                    powerTile.Enabled = true;
                    logger.Info("[LEDPowerTileService] Tile enabled");
                });
            }
            else
            {
                powerTile.Enabled = false;
                logger.Error("[LEDPowerTileService] No LED controllers are connected");
            }
        }

        private void OnRpcDisconnected()
        {
            executorService.Execute(() =>
            {
                deviceAddress = null;
                powerTile.Enabled = false;
                logger.Info("[LEDPowerTileService] Tile disabled");
            });
        }

        private void LedRpcService_OnPowerStateChanged(object sender, Network.EventArgs.OnPowerStateChangedEventArgs e)
        {
            SetPowerTileValue(e.PoweredOn);
        }

        private void NetworkService_ChannelConnected(object sender, EventArgs e)
        {
            OnRpcConnected();
        }

        private void NetworkService_ChannelLost(object sender, EventArgs e)
        {
            OnRpcDisconnected();
        }

        private void SetPowerTileValue(bool poweredOn)
        {
            powerTile.Value = poweredOn;
        }
    }
}
