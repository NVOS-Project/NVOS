using NVOS.Core.Services;
using NVOS.Network.Capabilities;
using NVOS.Network.Structs;
using NVOS.Network;
using NVOS.UI.Models.EventArgs;
using NVOS.UI.Models;
using NVOS.UI.Services;
using NVOS.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NVOS.Core;
using NVOS.Core.Services.Attributes;
using System.Linq;

namespace NVOS.LED
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(WorldUIService))]
    [ServiceDependency(typeof(QuickTileUIService))]
    [ServiceDependency(typeof(DeviceReflectionService))]
    [ServiceDependency(typeof(LedControllerRpcService))]
    [ServiceDependency(typeof(EmbeddedNetworkService))]
    [ServiceDependency(typeof(MainThreadExecutorService))]
    public class LedModeTileService : IService, IDisposable
    {
        private bool isDisposed;
        private Guid? deviceAddress;
        private Core.Logger.ILogger logger;
        private QuickTileUIService quickTileUIService;
        private DeviceReflectionService deviceReflection;
        private LedControllerRpcService ledRpcService;
        private EmbeddedNetworkService networkService;
        private MainThreadExecutorService executorService;

        private SwitchTile modeTile;

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
                modeTile.Interactable = false;

            networkService.ChannelConnected += NetworkService_ChannelConnected;
            networkService.ChannelLost += NetworkService_ChannelLost;

            ledRpcService.OnModeChanged += LedRpcService_OnModeChanged;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            OnRpcDisconnected();
            networkService.ChannelConnected -= NetworkService_ChannelConnected;
            networkService.ChannelLost -= NetworkService_ChannelLost;

            ledRpcService.OnModeChanged -= LedRpcService_OnModeChanged;

            modeTile.Dispose();

            quickTileUIService = null;
            deviceReflection = null;
            ledRpcService = null;
            networkService = null;
            executorService = null;
            isDisposed = true;
        }

        private void SetupTile()
        {
            modeTile = quickTileUIService.CreateSwitchTile("LED Mode");
            modeTile.DeactivatedText = "LED Mode: VIS";
            modeTile.ActivatedText = "LED Mode: IR";
            modeTile.DeactivatedColor = new Color32(50, 50, 50, 255);
            modeTile.ActivatedColor = new Color32(150, 0, 0, 255);
            modeTile.OnValueChanged += LedModeTile_OnValueChanged;
        }

        private void LedModeTile_OnValueChanged(object sender, SwitchTileValueChangedEventArgs e)
        {
            try
            {
                if (deviceAddress == null)
                    return;

                executorService.ExecuteAsync(async () =>
                {
                    if (e.IsOn)
                    {
                        await ledRpcService.SetModeAsync(deviceAddress.Value, Network.gRPC.LEDMode.Ir);
                    }
                    else
                    {
                        await ledRpcService.SetModeAsync(deviceAddress.Value, Network.gRPC.LEDMode.Vis);
                    }
                });

            }
            catch (Exception ex)
            {
                logger.Warn($"[LEDModeTileService] Failed to set new mode: {ex}");
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
                    SetModeTileValue(state.Mode);

                    deviceAddress = device.Address;
                    modeTile.Interactable = true;
                    logger.Info("[LEDModeTileService] Tile enabled");
                });
            }
            else
            {
                modeTile.Interactable = false;
                logger.Error("[LEDModeTileService] No LED controllers are connected");
            }
        }

        private void OnRpcDisconnected()
        {
            executorService.Execute(() =>
            {
                deviceAddress = null;
                modeTile.Interactable = false;
                logger.Info("[LEDModeTileService] Tile disabled");
            });
        }

        private void LedRpcService_OnModeChanged(object sender, Network.EventArgs.OnModeChangedEventArgs e)
        {
            SetModeTileValue(e.LEDMode);
        }

        private void NetworkService_ChannelConnected(object sender, EventArgs e)
        {
            OnRpcConnected();
        }

        private void NetworkService_ChannelLost(object sender, EventArgs e)
        {
            OnRpcDisconnected();
        }

        private void SetModeTileValue(Network.gRPC.LEDMode mode)
        {
            // false = VIS, true = IR
            switch (mode)
            {
                case Network.gRPC.LEDMode.Ir:
                    modeTile.IsOn = true;
                    break;
                case Network.gRPC.LEDMode.Vis:
                    modeTile.IsOn = false;
                    break;
            }
        }
    }
}
