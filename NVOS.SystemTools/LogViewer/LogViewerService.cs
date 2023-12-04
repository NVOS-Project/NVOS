using CircularBuffer;
using NVOS.Core;
using NVOS.Core.Logger;
using NVOS.Core.Logger.Enums;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.UI.Models;
using NVOS.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace NVOS.SystemTools.LogViewer
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
        private Panel filterPanel;
        private DropdownList levelFilterDropdown;
        private DropdownList tagFilterDropdown;

        private CircularBuffer<Log> logs;

        private Dictionary<LogLevel, bool> levelFilter;
        private Dictionary<string, bool> tagFilter;
        private bool filterByLevel;
        private bool filterByTag;

        private bool nextLog = false;

        public void Init()
        {
            worldUIService = ServiceLocator.Resolve<WorldUIService>();
            quickTileUIService = ServiceLocator.Resolve<QuickTileUIService>();
            logger = ServiceLocator.Resolve<BufferingLogger>();
            logger.OnLog += Logger_OnLog;

            logs = new CircularBuffer<Log>(32);
            levelFilter = new Dictionary<LogLevel, bool>();
            tagFilter = new Dictionary<string, bool>();

            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
                levelFilter.Add(level, false);

            filterByLevel = false;
            filterByTag = false;

            windowTile = quickTileUIService.CreateSwitchTile("Log Viewer");
            windowTile.OnValueChanged += WindowTile_OnValueChanged;

            SetupWindow();
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
            window = worldUIService.CreateWindow("Log Viewer", 30f, 20f);

            VerticalLayoutPanel mainPanel = new VerticalLayoutPanel("Main Panel");
            mainPanel.SizeScaleX = 1f;
            mainPanel.SizeScaleY = 1f;
            mainPanel.ReverseArrangement = true;
            window.GetContent().AddChild(mainPanel);

            logList = new VerticalLayoutPanel("Log List");
            logList.VerticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            logList.SizeScaleX = 1f;
            logList.ReverseArrangement = false;
            logList.PaddingBottom = 1;
            logList.ControlChildHeight = false;

            scrollView = new ScrollView("Log ScrollView", logList);
            scrollView.PreferredHeight = 18f;
            mainPanel.AddChild(scrollView);

            filterPanel = new Panel("Filter Panel");
            filterPanel.SizeScaleX = 1f;
            filterPanel.PreferredHeight = 2f;
            filterPanel.BackgroundColor = new Color32(50, 50, 50, 255);

            mainPanel.AddChild(filterPanel);

            levelFilterDropdown = new DropdownList("Level Filter Dropdown");
            levelFilterDropdown.DropdownSwitch.ActivatedText = "Filter by level";
            levelFilterDropdown.DropdownSwitch.DeactivatedText = "Filter by level";
            levelFilterDropdown.SizeOffsetX = 9f;
            levelFilterDropdown.SizeScaleY = 1f;
            levelFilterDropdown.ListHeight = 8f;

            tagFilterDropdown = new DropdownList("Tag Filter Dropdown");
            tagFilterDropdown.DropdownSwitch.ActivatedText = "Filter by tag";
            tagFilterDropdown.DropdownSwitch.DeactivatedText = "Filter by tag";
            tagFilterDropdown.SizeOffsetX = 9f;
            tagFilterDropdown.SizeScaleY = 1f;
            tagFilterDropdown.PositionOffsetX = 10f;
            tagFilterDropdown.ListHeight = 8f;
            filterPanel.AddChild(tagFilterDropdown);

            foreach (KeyValuePair<LogLevel, bool> kvp in levelFilter)
            {
                NVOS.UI.Models.Toggle levelToggle = new NVOS.UI.Models.Toggle($"{kvp.Key}");
                levelToggle.SizeOffsetY = 2f;
                levelToggle.OnValueChanged += (object sender, UI.Models.EventArgs.ToggleValueChangedEventArgs e) =>
                {
                    levelFilter[kvp.Key] = e.Value;
                    filterByLevel = levelFilter.Any(x => x.Value);
                    ReloadLogs();
                };
                levelFilterDropdown.AddListItem(levelToggle);
            }
            filterPanel.AddChild(levelFilterDropdown);

            window.OnWindowStateChanged += Window_OnWindowStateChanged;
            window.OnClose += Window_OnClose;

            foreach (Log log in logs)
            {
                AddLogPanel(log);
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

        private void ReloadLogs()
        {
            foreach (Control control in logList.controls)
                control.Dispose();

            logList.controls.Clear();

            foreach (Log log in logs)
                AddLogPanel(log);
        }

        private void AddLogPanel(Log log)
        {
            string[] newTags = log.Tags.Where(x => !tagFilter.ContainsKey(x)).ToArray();
            foreach (string tag in newTags)
            {
                tagFilter.Add(tag, false);
                NVOS.UI.Models.Toggle tagToggle = new NVOS.UI.Models.Toggle($"{tag}");
                tagToggle.Label.FontSize = 0.8f;
                tagToggle.SizeOffsetY = 2f;
                tagToggle.OnValueChanged += (object sender, UI.Models.EventArgs.ToggleValueChangedEventArgs e) =>
                {
                    tagFilter[tag] = e.Value;
                    filterByTag = tagFilter.Any(x => x.Value);
                    ReloadLogs();
                };
                tagFilterDropdown.AddListItem(tagToggle);
            }

            if (filterByLevel && !levelFilter[log.Level])
                return;

            if (filterByTag && !log.Tags.Any(x => tagFilter[x]))
                return;

            if (logList.controls.Count > 32)
            {
                Control pendingRemoval = logList.controls[0];
                logList.controls.RemoveAt(0);
                pendingRemoval.Dispose();
            }

            Color logColor;
            switch (log.Level)
            {
                case LogLevel.DEBUG:
                    logColor = new Color32(30, 30, 30, 255);
                    break;
                case LogLevel.INFO:
                    logColor = Color.white;
                    break;
                case LogLevel.WARN:
                    logColor = new Color32(255, 220, 50, 255);
                    break;
                case LogLevel.ERROR:
                    logColor = new Color32(145, 15, 27, 255);
                    break;
                default:
                    logColor = Color.white;
                    break;
            }

            HorizontalLayoutPanel logPanel = new HorizontalLayoutPanel("Log Item");
            logPanel.SizeScaleX = 1f;
            if (nextLog) {
                logPanel.BackgroundColor = new Color32(100, 100, 100, 255);
            }

            logPanel.VerticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Label logLabel = new Label("Log Label");
            logLabel.Text = log.Message;
            logLabel.Overflow = TMPro.TextOverflowModes.Ellipsis;
            logLabel.SizeScaleX = 1f;
            logLabel.SizeScaleY = 1f;
            logLabel.TextColor = logColor;
            logLabel.FontSize = 0.5f;
            logLabel.TextAlignment = TMPro.TextAlignmentOptions.BottomLeft;
            logLabel.Margin = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
            logPanel.AddChild(logLabel);

            logList.AddChild(logPanel);

            if (log.Message.Length > 120)
            {
                logPanel.PreferredHeight = 1.56f;

                SwitchButton logButton = new SwitchButton("Log Button");
                logButton.IgnoreLayout = true;
                logButton.SizeScaleX = 1f;
                logButton.SizeScaleY = 1f;
                logButton.ActivatedColor = Color.clear;
                logButton.DeactivatedColor = Color.clear;
                logButton.HighlightColor = Color.clear;
                logButton.ActivatedText = "";
                logButton.DeactivatedText = "";
                logButton.OnValueChanged += (object sender, UI.Models.EventArgs.SwitchButtonValueChangedEventArgs e) =>
                {
                    if (e.Value)
                    {
                        logPanel.GetRootObject().GetComponent<LayoutElement>().enabled = false;
                        logPanel.SizeOffsetX = 1f;
                    }
                    else
                    {
                        logPanel.GetRootObject().GetComponent<LayoutElement>().enabled = true;
                        logPanel.PreferredHeight = 1.56f;
                    }
                };
                logPanel.AddChild(logButton);
            }

            nextLog = !nextLog;
        }

        private void Logger_OnLog(object sender, Core.Logger.EventArgs.LogEventArgs e)
        {
            Log log = new Log(e.Level, e.Message, e.Tags);

            logs.PushBack(log);

            if (window == null)
                return;

            AddLogPanel(log);
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
