using NVOS.Core;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.UI.Models;
using NVOS.UI.Services;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NVOS.SystemTools
{
    [ServiceType(Core.Services.Enums.ServiceType.Singleton)]
    [ServiceDependency(typeof(WorldUIService))]
    [ServiceDependency(typeof(QuickTileUIService))]
    public class LogViewerService : IService, IDisposable
    {
        private BufferingLogger logger;
        private WorldUIService worldUIService;
        private QuickTileUIService quickTileUIService;

        private Window3D window;
        private SwitchTile windowTile;
        private ScrollView scrollView;
        private VerticalLayoutPanel logList;

        private List<KeyValuePair<LogLevel, string>> logs;

        public void Init()
        {
            worldUIService = ServiceLocator.Resolve<WorldUIService>();
            quickTileUIService = ServiceLocator.Resolve<QuickTileUIService>();
            logger = ServiceLocator.Resolve<BufferingLogger>();
            logger.OnLog += Logger_OnLog;
            logs = new List<KeyValuePair<LogLevel, string>>();

            windowTile = quickTileUIService.CreateSwitchTile("Log Viewer");
            windowTile.OnValueChanged += WindowTile_OnValueChanged;
        }

        public void Dispose()
        {
            windowTile.OnValueChanged -= WindowTile_OnValueChanged;
            logger.OnLog -= Logger_OnLog;

            if (window != null)
                window.Close();

            quickTileUIService = null;
            windowTile.Dispose();
            worldUIService = null;
            logger = null;
            logs = null;
        }

        private void SetupWindow()
        {
            window = worldUIService.CreateWindow("Log Viewer", 60f, 40f);
            logList = new VerticalLayoutPanel("Log List");
            logList.VerticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            logList.SizeScaleX = 1f;
            logList.ReverseArrangement = true;
            logList.PaddingBottom = 1;
            logList.ControlChildHeight = false;

            scrollView = new ScrollView("Log ScrollView", logList);
            scrollView.SizeOffsetX = 60f;
            scrollView.SizeOffsetY = window.Height * 0.9f;
            window.GetContent().AddChild(scrollView);
            window.OnWindowStateChanged += Window_OnWindowStateChanged;
            window.OnClose += Window_OnClose;

            foreach (KeyValuePair<LogLevel, string> entry in logs)
            {
                AddLogPanel(entry.Key, entry.Value);
            }
        }

        private void Window_OnWindowStateChanged(object sender, UI.Models.EventArgs.WindowStateChangedEventArgs e)
        {
            if (e.State == UI.Models.Enums.WindowState.Normal)
                windowTile.Value = true;
            else
                windowTile.Value = false;
        }

        private void Window_OnClose(object sender, UI.Models.EventArgs.WindowEventArgs e)
        {
            windowTile.Value = false;
            window = null;
        }

        private void AddLogPanel(LogLevel logLevel, string message)
        {
            if (logList.controls.Count > 32)
            {
                Control pendingRemoval = logList.controls[0];
                logList.controls.RemoveAt(0);
                pendingRemoval.Dispose();
            }

            Color logColor;
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    logColor = new Color32(40, 130, 0, 255);
                    break;
                case LogLevel.INFO:
                    logColor = Color.clear;
                    break;
                case LogLevel.WARN:
                    logColor = new Color32(130, 100, 0, 255);
                    break;
                case LogLevel.ERROR:
                    logColor = new Color32(130, 0, 0, 255);
                    break;
                default:
                    logColor = Color.clear;
                    break;
            }

            Panel logPanel = new Panel("Log Item");
            logPanel.BackgroundColor = logColor;
            logPanel.SizeOffsetY = 4.4f;

            Label logLabel = new Label("Log Label");
            logLabel.Text = message;
            logLabel.SizeScaleX = 1f;
            logLabel.SizeScaleY = 1f;
            logLabel.TextColor = Color.white;
            logLabel.FontSize = 1f;
            logLabel.TextAlignment = TMPro.TextAlignmentOptions.BottomLeft;
            logLabel.Margin = new Vector4(1f, 1f, 1f, 1f);
            logLabel.Overflow = TMPro.TextOverflowModes.Ellipsis;
            logPanel.AddChild(logLabel);

            logList.AddChild(logPanel);
        }

        private void Logger_OnLog(object sender, Core.Logger.EventArgs.LogEventArgs e)
        {
            logs.Add(new KeyValuePair<LogLevel, string>(e.Level, e.Message));
            if (logs.Count > 32)
                logs.RemoveAt(0);

            if (window == null)
                return;

            AddLogPanel(e.Level, e.Message);
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
            if (!e.Value)
                window.Hide();
        }
    }
}
