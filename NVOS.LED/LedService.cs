using NVOS.Core;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.UI.Models;
using NVOS.UI.Services;

namespace NVOS.LED
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(WorldUIService))]
    [ServiceDependency(typeof(QuickTileUIService))]
    public class LedService : IService
    {
        private WorldUIService worldUIService;
        private QuickTileUIService quickTileUIService;
        private Window3D window;

        private SwitchTile windowTile;
        private SwitchTile ledPowerTile;
        private SwitchTile ledModeTile;

        private SwitchButton ledPowerSwitch;
        private SwitchButton ledModeSwitch;
        private HorizontalSlider brightnessSlider;

        public bool Init()
        {
            worldUIService = ServiceLocator.Resolve<WorldUIService>();
            quickTileUIService = ServiceLocator.Resolve<QuickTileUIService>();

            SetupWindow();
            SetupTiles();

            return true;
        }

        private void SetupWindow()
        {
            window = worldUIService.CreateWindow("LED settings", 60f, 40f);

            HorizontalLayoutPanel ledPowerPanel = new HorizontalLayoutPanel("Light Panel");
            ledPowerPanel.SizeOffsetX = 40f;
            ledPowerPanel.SizeOffsetY = 5f;
            ledPowerPanel.Spacing = 5f;
            ledPowerPanel.PositionOffsetX = (window.Width - ledPowerPanel.SizeOffsetX) / 2;
            ledPowerPanel.PositionScaleY = 0.15f;
            window.GetContent().AddChild(ledPowerPanel);

            Label ledPowerLabel = new Label("Light Label");
            ledPowerLabel.PreferredWidth = 20f;
            ledPowerLabel.Text = "LED power";
            ledPowerLabel.FontSize = 2f;
            ledPowerLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            ledPowerPanel.AddChild(ledPowerLabel);

            ledPowerSwitch = new SwitchButton("Light Switch");
            ledPowerSwitch.PreferredWidth = 5f;
            ledPowerSwitch.DeactivatedColor = Color.black;
            ledPowerSwitch.ActivatedColor = Color.gray;
            ledPowerSwitch.DeactivatedText = "OFF";
            ledPowerSwitch.ActivatedText = "ON";
            ledPowerSwitch.FontSize = 2f;
            ledPowerSwitch.DeactivatedColor = new Color32(150, 30, 30, 255);
            ledPowerSwitch.ActivatedColor = new Color32(30, 150, 50, 255);
            ledPowerSwitch.OnValueChanged += LedPowerSwitch_OnValueChanged;
            ledPowerPanel.AddChild(ledPowerSwitch);

            HorizontalLayoutPanel ledModePanel = new HorizontalLayoutPanel("LED Panel");
            ledModePanel.SizeOffsetX = 40f;
            ledModePanel.SizeOffsetY = 5f;
            ledModePanel.Spacing = 5f;
            ledModePanel.PositionOffsetX = (window.Width - ledModePanel.SizeOffsetX) / 2;
            ledModePanel.PositionScaleY = 0.45f;
            window.GetContent().AddChild(ledModePanel);

            Label ledModeLabel = new Label("LED Label");
            ledModeLabel.PreferredWidth = 20f;
            ledModeLabel.Text = "LED mode";
            ledModeLabel.FontSize = 2f;
            ledModeLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            ledModePanel.AddChild(ledModeLabel);

            ledModeSwitch = new SwitchButton("LED Switch");
            ledModeSwitch.PreferredWidth = 5f;
            ledModeSwitch.DeactivatedColor = Color.black;
            ledModeSwitch.ActivatedColor = Color.gray;
            ledModeSwitch.DeactivatedText = "VIS";
            ledModeSwitch.ActivatedText = "IR";
            ledModeSwitch.FontSize = 2f;
            ledModeSwitch.OnValueChanged += LedModeSwitch_OnValueChanged;
            ledModeSwitch.DeactivatedColor = new Color32(50, 50, 50, 255);
            ledModeSwitch.ActivatedColor = new Color32(150, 0, 0, 255);
            ledModePanel.AddChild(ledModeSwitch);

            HorizontalLayoutPanel brightnessPanel = new HorizontalLayoutPanel("Brightness Panel");
            brightnessPanel.SizeOffsetX = 40f;
            brightnessPanel.SizeOffsetY = 5f;
            brightnessPanel.Spacing = 5f;
            brightnessPanel.PositionOffsetX = (window.Width - brightnessPanel.SizeOffsetX) / 2;
            brightnessPanel.PositionScaleY = 0.75f;
            window.GetContent().AddChild(brightnessPanel);

            Label brightnessLabel = new Label("Brightness Label");
            brightnessLabel.Text = "Brightness";
            brightnessLabel.PreferredWidth = 10f;
            brightnessLabel.SizeOffsetY = 5f;
            brightnessLabel.FontSize = 2f;
            brightnessLabel.TextAlignment = TMPro.TextAlignmentOptions.Left;
            brightnessPanel.AddChild(brightnessLabel);

            Panel sliderPanel = new Panel();
            sliderPanel.PreferredWidth = 25f;
            brightnessPanel.AddChild(sliderPanel);

            brightnessSlider = new HorizontalSlider("Brightness Slider");
            brightnessSlider.SizeOffsetX = 25f;
            brightnessSlider.SizeOffsetY = 2f;
            brightnessSlider.PositionOffsetY = 1.5f;
            brightnessSlider.OnValueChanged += BrightnessSlider_OnValueChanged;
            sliderPanel.AddChild(brightnessSlider);

            Panel warningPanel = new Panel("Warning Panel");
            warningPanel.BackgroundColor = new Color32(255, 100, 100, 255);
            warningPanel.SizeScaleX = 1f;
            warningPanel.SizeOffsetY = 3f;
            window.GetContent().AddChild(warningPanel);

            Label warningLabel = new Label("Warning Label");
            warningLabel.FontSize = 2;
            warningLabel.Text = "LEDController not detected!";
            warningLabel.SizeScaleX = 1f;
            warningLabel.SizeScaleY = 1f;
            warningLabel.TextColor = Color.white;
            warningPanel.AddChild(warningLabel);

            warningPanel.IsVisible = false;
            window.Hide();

            window.OnWindowStateChanged += Window_OnWindowStateChanged;
            window.OnClose += Window_OnClose;
        }

        private void Window_OnWindowStateChanged(object sender, UI.Models.EventArgs.WindowStateChangedEventArgs e)
        {
            if (e.State == UI.Models.Enums.WindowState.Normal)
                windowTile.IsOn = true;

            if (e.State == UI.Models.Enums.WindowState.Hidden)
                windowTile.IsOn = false;
        }

        private void Window_OnClose(object sender, UI.Models.EventArgs.WindowEventArgs e)
        {
            windowTile.IsOn = false;
            window = null;
        }

        private void LedPowerSwitch_OnValueChanged(object sender, UI.Models.EventArgs.SwitchButtonValueChangedEventArgs e)
        {
            SetLedPower(e.IsOn);
        }

        private void LedModeSwitch_OnValueChanged(object sender, UI.Models.EventArgs.SwitchButtonValueChangedEventArgs e)
        {
            SetLedMode(e.IsOn);
        }

        private void BrightnessSlider_OnValueChanged(object sender, UI.Models.EventArgs.SliderValueChangedEventArgs e)
        {
            SetBrightness(e.Value);
        }

        private void SetupTiles()
        {
            windowTile = quickTileUIService.CreateSwitchTile("LED settings");
            windowTile.ActivatedColor = new Color32(100, 100, 100, 255);
            windowTile.OnValueChanged += WindowTile_OnValueChanged;

            ledPowerTile = quickTileUIService.CreateSwitchTile("LED Power");
            ledPowerTile.DeactivatedText = "LED Power: OFF";
            ledPowerTile.ActivatedText = "LED Power: ON";
            ledPowerTile.DeactivatedColor = new Color32(150, 30, 30, 255);
            ledPowerTile.ActivatedColor = new Color32(30, 150, 50, 255);
            ledPowerTile.OnValueChanged += LedPowerTile_OnValueChanged;

            ledModeTile = quickTileUIService.CreateSwitchTile("LED Mode");
            ledModeTile.DeactivatedText = "LED Mode: VIS";
            ledModeTile.ActivatedText = "LED Mode: IR";
            ledModeTile.DeactivatedColor = new Color32(50, 50, 50, 255);
            ledModeTile.ActivatedColor = new Color32(150, 0, 0, 255);
            ledModeTile.OnValueChanged += LedModeTile_OnValueChanged;
        }

        private void WindowTile_OnValueChanged(object sender, UI.Models.EventArgs.SwitchTileValueChangedEventArgs e)
        {
            if (window == null)
            {
                SetupWindow();
                return;
            }

            if (e.IsOn)
                window.Show();
            else
                window.Hide();
        }

        private void LedPowerTile_OnValueChanged(object sender, UI.Models.EventArgs.SwitchTileValueChangedEventArgs e)
        {
            SetLedPower(e.IsOn);
        }

        private void LedModeTile_OnValueChanged(object sender, UI.Models.EventArgs.SwitchTileValueChangedEventArgs e)
        {
            SetLedMode(e.IsOn);
        }

        private void SetLedPower(bool value)
        {
            ledPowerSwitch.IsOn = value;
            ledPowerTile.IsOn = value;
        }

        private void SetLedMode(bool value)
        {
            ledModeSwitch.IsOn = value;
            ledModeTile.IsOn = value;
        }

        private void SetBrightness(float value)
        {
            Debug.Log("setting brightness to " + value);
        }
    }
}
