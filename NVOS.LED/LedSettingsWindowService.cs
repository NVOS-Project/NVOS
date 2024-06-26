﻿using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.UI.Models;
using NVOS.UI.Services;
using NVOS.Network.Capabilities;
using System;
using UnityEngine;
using NVOS.Network;
using System.Linq;
using NVOS.Network.Structs;
using static UnityEngine.Scripting.GarbageCollector;
using NVOS.Unity;

namespace NVOS.LED
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(WorldUIService))]
    [ServiceDependency(typeof(QuickTileUIService))]
    [ServiceDependency(typeof(DeviceReflectionService))]
    [ServiceDependency(typeof(LedControllerRpcService))]
    [ServiceDependency(typeof(EmbeddedNetworkService))]
    [ServiceDependency(typeof(MainThreadExecutorService))]
    public class LedSettingsWindowService : IService, IDisposable
    {
        private bool isDisposed;
        private Guid? deviceAddress;
        private Core.Logger.ILogger logger;
        private WorldUIService worldUIService;
        private QuickTileUIService quickTileUIService;
        private DeviceReflectionService deviceReflection;
        private LedControllerRpcService ledRpcService;
        private EmbeddedNetworkService networkService;
        private MainThreadExecutorService executorService;
        
        private Window3D window;
        private SwitchTile windowTile;

        private SwitchButton ledPowerSwitch;
        private SwitchButton ledModeSwitch;
        private HorizontalSlider brightnessSlider;
        private Panel warningPanel;

        public void Init()
        {
            logger = ServiceLocator.Resolve<Core.Logger.ILogger>();
            worldUIService = ServiceLocator.Resolve<WorldUIService>();
            quickTileUIService = ServiceLocator.Resolve<QuickTileUIService>();
            deviceReflection = ServiceLocator.Resolve<DeviceReflectionService>();
            ledRpcService = ServiceLocator.Resolve<LedControllerRpcService>();
            networkService = ServiceLocator.Resolve<EmbeddedNetworkService>();
            executorService = ServiceLocator.Resolve<MainThreadExecutorService>();

            SetupWindow();
            SetupTile();

            if (networkService.IsConnected)
                OnRpcConnected();

            networkService.ChannelConnected += NetworkService_ChannelConnected;
            networkService.ChannelLost += NetworkService_ChannelLost;

            ledRpcService.OnBrightnessChanged += LedRpcService_OnBrightnessChanged;
            ledRpcService.OnModeChanged += LedRpcService_OnModeChanged;
            ledRpcService.OnPowerStateChanged += LedRpcService_OnPowerStateChanged;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            OnRpcDisconnected();
            networkService.ChannelConnected -= NetworkService_ChannelConnected;
            networkService.ChannelLost -= NetworkService_ChannelLost;

            ledRpcService.OnBrightnessChanged -= LedRpcService_OnBrightnessChanged;
            ledRpcService.OnModeChanged -= LedRpcService_OnModeChanged;
            ledRpcService.OnPowerStateChanged -= LedRpcService_OnPowerStateChanged;

            window.Close();
            windowTile.Dispose();

            worldUIService = null;
            quickTileUIService = null;
            deviceReflection = null;
            ledRpcService = null;
            networkService = null;
            executorService = null;
            isDisposed = true;
        }

        private void OnRpcConnected()
        {
            Device device = deviceReflection.GetDevicesWithCapability(Network.gRPC.CapabilityId.Ledcontroller).FirstOrDefault();
            if (device != null)
            {
                LEDState state = ledRpcService.GetState(device.Address);

                executorService.Execute(() =>
                {
                    SetPowerControlValue(state.PoweredOn);
                    SetModeControlValue(state.Mode);
                    SetBrightnessControlValue(state.Brightness);

                    deviceAddress = device.Address;
                    warningPanel.IsVisible = false;
                    logger.Info("[LEDSettingsWindowService] Controls enabled");
                });
            }
            else
            {
                logger.Error("[LEDSettingsWindowService] No LED controllers are connected");
            }
        }

        private void OnRpcDisconnected()
        {
            executorService.Execute(() =>
            {
                deviceAddress = null;
                warningPanel.IsVisible = true;
                logger.Info("[LEDSettingsWindowService] Controls disabled");
            });
        }

        private void LedRpcService_OnPowerStateChanged(object sender, Network.EventArgs.OnPowerStateChangedEventArgs e)
        {
            SetPowerControlValue(e.PoweredOn);
        }

        private void LedRpcService_OnModeChanged(object sender, Network.EventArgs.OnModeChangedEventArgs e)
        {
            SetModeControlValue(e.LEDMode);
        }

        private void LedRpcService_OnBrightnessChanged(object sender, Network.EventArgs.OnBrightnessChangedEventArgs e)
        {
            SetBrightnessControlValue(e.Brightness);
        }

        private void NetworkService_ChannelConnected(object sender, EventArgs e)
        {
            OnRpcConnected();
        }

        private void NetworkService_ChannelLost(object sender, EventArgs e)
        {
            OnRpcDisconnected();
        }

        private void SetupWindow()
        {
            window = worldUIService.CreateWindow("LED settings", 30f, 20f);

            HorizontalLayoutPanel ledPowerPanel = new HorizontalLayoutPanel("Light Panel");
            ledPowerPanel.SizeOffsetX = 20f;
            ledPowerPanel.SizeOffsetY = 3f;
            ledPowerPanel.Spacing = 3f;
            ledPowerPanel.PositionOffsetX = (window.Width - ledPowerPanel.SizeOffsetX) / 2;
            ledPowerPanel.PositionScaleY = 0.15f;
            window.GetContent().AddChild(ledPowerPanel);

            Label ledPowerLabel = new Label("Light Label");
            ledPowerLabel.PreferredWidth = 12f;
            ledPowerLabel.Text = "LED power";
            ledPowerLabel.FontSize = 1.5f;
            ledPowerLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            ledPowerPanel.AddChild(ledPowerLabel);

            ledPowerSwitch = new SwitchButton("Light Switch");
            ledPowerSwitch.PreferredWidth = 3f;
            ledPowerSwitch.DeactivatedColor = Color.black;
            ledPowerSwitch.ActivatedColor = Color.gray;
            ledPowerSwitch.DeactivatedText = "OFF";
            ledPowerSwitch.ActivatedText = "ON";
            ledPowerSwitch.FontSize = 1f;
            ledPowerSwitch.DeactivatedColor = new Color32(150, 30, 30, 255);
            ledPowerSwitch.ActivatedColor = new Color32(30, 150, 50, 255);
            ledPowerSwitch.OnValueChanged += LedPowerSwitch_OnValueChanged;
            ledPowerPanel.AddChild(ledPowerSwitch);

            HorizontalLayoutPanel ledModePanel = new HorizontalLayoutPanel("LED Panel");
            ledModePanel.SizeOffsetX = 20f;
            ledModePanel.SizeOffsetY = 3f;
            ledModePanel.Spacing = 3f;
            ledModePanel.PositionOffsetX = (window.Width - ledModePanel.SizeOffsetX) / 2;
            ledModePanel.PositionScaleY = 0.45f;
            window.GetContent().AddChild(ledModePanel);

            Label ledModeLabel = new Label("LED Label");
            ledModeLabel.PreferredWidth = 12f;
            ledModeLabel.Text = "LED mode";
            ledModeLabel.FontSize = 1.5f;
            ledModeLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            ledModePanel.AddChild(ledModeLabel);

            ledModeSwitch = new SwitchButton("LED Switch");
            ledModeSwitch.PreferredWidth = 3f;
            ledModeSwitch.DeactivatedColor = Color.black;
            ledModeSwitch.ActivatedColor = Color.gray;
            ledModeSwitch.DeactivatedText = "VIS";
            ledModeSwitch.ActivatedText = "IR";
            ledModeSwitch.FontSize = 1f;
            ledModeSwitch.OnValueChanged += LedModeSwitch_OnValueChanged;
            ledModeSwitch.DeactivatedColor = new Color32(50, 50, 50, 255);
            ledModeSwitch.ActivatedColor = new Color32(150, 0, 0, 255);
            ledModePanel.AddChild(ledModeSwitch);

            HorizontalLayoutPanel brightnessPanel = new HorizontalLayoutPanel("Brightness Panel");
            brightnessPanel.SizeOffsetX = 25f;
            brightnessPanel.SizeOffsetY = 3f;
            brightnessPanel.Spacing = 3f;
            brightnessPanel.PositionOffsetX = (window.Width - brightnessPanel.SizeOffsetX) / 2;
            brightnessPanel.PositionScaleY = 0.75f;
            window.GetContent().AddChild(brightnessPanel);

            Label brightnessLabel = new Label("Brightness Label");
            brightnessLabel.Text = "Brightness";
            brightnessLabel.PreferredWidth = 4.5f;
            brightnessLabel.SizeOffsetY = 3f;
            brightnessLabel.FontSize = 1f;
            brightnessLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            brightnessPanel.AddChild(brightnessLabel);

            Panel sliderPanel = new Panel();
            sliderPanel.PreferredWidth = 15f;
            brightnessPanel.AddChild(sliderPanel);

            brightnessSlider = new HorizontalSlider("Brightness Slider");
            brightnessSlider.SizeOffsetX = 15f;
            brightnessSlider.SizeOffsetY = 1f;
            brightnessSlider.PositionOffsetY = 1;
            brightnessSlider.OnValueChanged += BrightnessSlider_OnValueChanged;
            sliderPanel.AddChild(brightnessSlider);

            warningPanel = new Panel("Warning Panel");
            warningPanel.BackgroundColor = new Color32(255, 100, 100, 255);
            warningPanel.SizeScaleX = 1f;
            warningPanel.SizeOffsetY = 2f;
            window.GetContent().AddChild(warningPanel);

            Label warningLabel = new Label("Warning Label");
            warningLabel.FontSize = 1;
            warningLabel.Text = "LED controller is unavailable";
            warningLabel.SizeScaleX = 1f;
            warningLabel.SizeScaleY = 1f;
            warningLabel.TextColor = Color.white;
            warningPanel.AddChild(warningLabel);

            warningPanel.IsVisible = true;
            window.Hide();

            window.OnWindowStateChanged += Window_OnWindowStateChanged;
            window.OnClose += Window_OnClose;
        }
        private void SetupTile()
        {
            windowTile = quickTileUIService.CreateSwitchTile("LED Settings");
            windowTile.ActivatedColor = new Color32(100, 100, 100, 255);
            windowTile.OnValueChanged += WindowTile_OnValueChanged;
        }

        private void Window_OnWindowStateChanged(object sender, UI.Models.EventArgs.WindowStateChangedEventArgs e)
        {
            if (e.State == UI.Models.Enums.WindowState.Normal)
                windowTile.Value = true;

            if (e.State == UI.Models.Enums.WindowState.Hidden)
                windowTile.Value = false;
        }

        private void Window_OnClose(object sender, UI.Models.EventArgs.WindowEventArgs e)
        {
            windowTile.Value = false;
            window = null;
        }

        private void SetPowerControlValue(bool poweredOn)
        {
            ledPowerSwitch.Value = poweredOn;
        }

        private void SetModeControlValue(Network.gRPC.LEDMode mode)
        {
            // false = VIS, true = IR
            switch (mode)
            {
                case Network.gRPC.LEDMode.Ir:
                    ledModeSwitch.Value = true;
                    break;
                case Network.gRPC.LEDMode.Vis:
                    ledModeSwitch.Value = false;
                    break;
            }
        }

        private void SetBrightnessControlValue(float brightness)
        {
            brightnessSlider.Value = Mathf.Clamp01(brightness);
        }

        private void LedPowerSwitch_OnValueChanged(object sender, UI.Models.EventArgs.SwitchButtonValueChangedEventArgs e)
        {
            try
            {
                if (deviceAddress == null)
                    return;

                executorService.ExecuteAsync(async() =>
                {
                    await ledRpcService.SetPowerStateAsync(deviceAddress.Value, e.Value);
                });
                
            }
            catch(Exception ex)
            {
                logger.Warn($"[LEDSettingsWindowService] Failed to set new power state: {ex}");
            }
        }

        private void LedModeSwitch_OnValueChanged(object sender, UI.Models.EventArgs.SwitchButtonValueChangedEventArgs e)
        {
            try
            {
                if (deviceAddress == null)
                    return;

                executorService.ExecuteAsync(async () =>
                {
                    if (e.Value)
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
                logger.Warn($"[LEDSettingsWindowService] Failed to set new mode: {ex}");
            }
        }

        private void BrightnessSlider_OnValueChanged(object sender, UI.Models.EventArgs.SliderValueChangedEventArgs e)
        {
            try
            {
                if (deviceAddress == null)
                    return;

                executorService.ExecuteAsync(async () =>
                {
                    await ledRpcService.SetBrightnessAsync(deviceAddress.Value, e.Value);
                });
                
            }
            catch (Exception ex)
            {
                logger.Warn($"[LEDSettingsWindowService] Failed to set new brightness: {ex}");
            }
        }

        private void WindowTile_OnValueChanged(object sender, UI.Models.EventArgs.SwitchTileValueChangedEventArgs e)
        {
            if (window == null)
            {
                SetupWindow();
                return;
            }

            if (e.Value)
                window.Show();
            else
                window.Hide();
        }
    }
}
