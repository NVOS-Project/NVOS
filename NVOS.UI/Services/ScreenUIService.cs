using NVOS.Core;
using NVOS.Core.Database;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using NVOS.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(UpdateProviderService))]
    [ServiceDependency(typeof(WorldUIService))]
    public class ScreenUIService : IService, IDisposable
    {
        private Dictionary<Widget, int> widgets;

        private UpdateProviderService updateProvider;
        private WorldUIService worldUIService;
        private bool isDisposed;

        private Window3D widgetWindow;
        private Window2D hudWindow;
        private GridLayoutPanel widgetGrid;
        private GridLayoutPanel hudGrid;
        private VerticalLayoutPanel widgetList;
        private Panel gridWrap;

        private float hudWidth;
        private float hudHeight;
        private int gridWidth;
        private int gridHeight;
        private float tileWidth;
        private float tileHeight;

        private int maxWidgetWidth;
        private int maxWidgetHeight;

        private Widget selectedWidget;
        private SwitchButton selectedWidgetButton;
        private Panel wrapPanel;

        public void Init()
        {
            updateProvider = ServiceLocator.Resolve<UpdateProviderService>();
            worldUIService = ServiceLocator.Resolve<WorldUIService>();
            IDatabaseService databaseService = ServiceLocator.Resolve<IDatabaseService>();
            DbCollection collection = databaseService.GetCollection("screen_ui");

            updateProvider.OnLateUpdate += UpdateProvider_OnLateUpdate;

            hudWidth = (float)collection.ReadOrDefault("hudWidth", 30f);
            hudHeight = (float)collection.ReadOrDefault("hudHeight", 20f);
            gridWidth = (int)collection.ReadOrDefault("gridWidth", 8);
            gridHeight = (int)collection.ReadOrDefault("gridHeight", 6);
            Debug.LogError("here?");
            tileWidth = hudWidth / gridWidth;
            tileHeight = hudHeight / gridHeight;
            Debug.LogError("here?*");

            collection.Write("maxWidgetWidth", 4);
            collection.Write("maxWidgetHeight", 4);

            maxWidgetWidth = (int)collection.ReadOrDefault("maxWidgetWidth", 4);
            maxWidgetHeight = (int)collection.ReadOrDefault("maxWidgetHeight", 4);

            Debug.LogError("here??");

            selectedWidget = null;
            selectedWidgetButton = null;


            widgets = new Dictionary<Widget, int>();
            Debug.LogError("here???");
            SetupHud();
            Debug.LogError("here????");
            SetupWindow();
            Debug.LogError("here?????");
        }

        public void Dispose()
        {
            
        }

        private void SetupHud()
        {
            hudWindow = new Window2D("HUD", hudWidth, hudHeight);
            hudWindow.GetRootObject().transform.SetParent(Camera.main.transform);
            hudWindow.GetRootObject().transform.localPosition = new Vector3(0, 0, 1);
            hudWindow.GetContent().BackgroundColor = Color.clear;

            hudGrid = new GridLayoutPanel("HUD Grid");
            hudWindow.GetContent().AddChild(hudGrid);
            hudGrid.SizeScaleX = 1f;
            hudGrid.SizeScaleY = 1f;
            hudGrid.Constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
            hudGrid.ConstraintCount = gridWidth;
            hudGrid.StartCorner = UnityEngine.UI.GridLayoutGroup.Corner.UpperLeft;
            hudGrid.CellSize = new Vector2(tileWidth, tileHeight);

            for (int i = 0; i < gridWidth * gridHeight; i++)
            {
                Panel gridPanel = new Panel("Grid Panel");
                gridPanel.BackgroundColor = Color.clear;
                hudGrid.AddChild(gridPanel);
            }
        }

        private void SetupWindow()
        {
            Widget widget1 = CreateWidget("1", 3, 2);
            widget1.PreviewColor = Color.red;
            widget1.BackgroundColor = widget1.PreviewColor;

            Widget widget2 = CreateWidget("2", 4, 3);
            widget2.PreviewColor = Color.blue;
            widget2.BackgroundColor = widget2.PreviewColor;

            Widget widget3 = CreateWidget("3", 2, 4);
            widget3.PreviewColor = Color.yellow;
            widget3.BackgroundColor = widget3.PreviewColor;

            Widget widget4 = CreateWidget("4", 3, 3);
            widget4.PreviewColor = Color.green;
            widget4.BackgroundColor = widget4.PreviewColor;

            Widget widget5 = CreateWidget("5", 4, 2);
            widget5.PreviewColor = Color.gray;
            widget5.BackgroundColor = widget5.PreviewColor;



            widgetWindow = worldUIService.CreateWindow("Widget Window", 30f, 20f);

            widgetList = new VerticalLayoutPanel("Widget Panel");
            widgetList.VerticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            widgetList.ControlChildHeight = false;
            widgetList.ControlChildWidth = false;
            widgetList.Spacing = 1f;
            widgetList.PaddingLeft = 1;
            widgetList.PaddingRight = 1;
            widgetList.PaddingTop = 1;
            widgetList.PaddingBottom = 1;
            widgetList.SizeScaleX = 1f;
            widgetList.ChildAlignment = TextAnchor.MiddleCenter;

            ScrollView widgetScrollview = new ScrollView("Widget List", widgetList);
            widgetWindow.GetContent().AddChild(widgetScrollview);
            widgetScrollview.SizeScaleX = 0.4f;
            widgetScrollview.SizeScaleY = 1f;

            wrapPanel = new Panel("Wrap Panel");
            widgetWindow.GetContent().AddChild(wrapPanel);
            wrapPanel.PositionScaleX = 0.4f;
            wrapPanel.SizeScaleX = 0.6f;
            wrapPanel.SizeScaleY = 1f;

            gridWrap = new Panel("Grid Wrap");
            wrapPanel.AddChild(gridWrap);
            gridWrap.SizeOffsetX = gridWidth * 2;
            gridWrap.SizeOffsetY = gridHeight * 2;
            gridWrap.PositionScaleX = 1 - (gridWrap.SizeOffsetX / (widgetWindow.Width * wrapPanel.SizeScaleX)) / 2;
            gridWrap.PositionScaleY = 1 - (gridWrap.SizeOffsetY / (widgetWindow.Height * wrapPanel.SizeScaleY)) / 2;

            widgetGrid = new GridLayoutPanel("Widget Grid");
            gridWrap.AddChild(widgetGrid);
            widgetGrid.SizeScaleX = 1f;
            widgetGrid.SizeScaleY = 1f;
            widgetGrid.CellSize = new Vector2(2f, 2f);

            for (int i = 0; i < gridWidth * gridHeight; i++)
            {
                NVOS.UI.Models.Button gridButton = new NVOS.UI.Models.Button();
                gridButton.BackgroundColor = Color.white;
                gridButton.Label.Text = "";
                gridButton.GetRootObject().AddComponent<Image>();
                Outline outline = gridButton.GetRootObject().AddComponent<Outline>();
                outline.effectDistance = new Vector2(0.05f, 0.05f);
                outline.effectColor = Color.black;
                widgetGrid.AddChild(gridButton);

                gridButton.OnClick += (sender, e) =>
                {
                    if (selectedWidget == null)
                        return;

                    ShowSelectedWidget(widgetGrid.controls.IndexOf((Control)sender));
                    Debug.LogError("selected widget sender gets cast");
                };
            }

            foreach (KeyValuePair<Widget, int> kvp in widgets)
            {
                if (kvp.Value == -1)
                {
                    CreateWidgetButton(kvp.Key);
                }
            }
        }

        private void CreateWidgetButton(Widget widget)
        {
            SwitchButton switchButton = new SwitchButton($"{widget.Name}\n{widget.TileWidth}x{widget.TileHeight}");
            switchButton.DeactivatedColor = widget.PreviewColor;
            switchButton.Label.FontSize = 0.6f;
            switchButton.Label.TextAlignment = TMPro.TextAlignmentOptions.Center;
            switchButton.SizeOffsetX = widget.TileWidth * 2;
            switchButton.SizeOffsetY = widget.TileHeight * 2;
            widgetList.AddChild(switchButton);

            switchButton.OnValueChanged += (sender, e) =>
            {
                if (!e.Value)
                {
                    UnselectWidget();
                    return;
                }

                if (widget.IsVisible)
                {
                    HideWidget(widget, (SwitchButton)sender);
                    return;
                }

                foreach (SwitchButton button in widgetList.controls)
                {
                    if (!button.Value || button == sender)
                        continue;
                    button.Value = false;
                }
                SelectWidget(widget, (SwitchButton)sender);
                Debug.LogError("select widget sender gets cast");
            };
        }

        public Widget CreateWidget(string name, int width, int height) {
            if (width > maxWidgetWidth || height > maxWidgetHeight)
                throw new Exception("Widget size is out of range!");

            Widget widget = new Widget(name, width, height);
            widget.IsVisible = false;
            hudWindow.GetContent().AddChild(widget);
            widget.SizeOffsetX = width * tileWidth;
            widget.SizeOffsetY = height * tileHeight;
            widgets.Add(widget, -1);

            return widget;
        }

        private void SelectWidget(Widget widget, SwitchButton button)
        {
            selectedWidget = widget;
            selectedWidgetButton = button;
        }

        private void UnselectWidget()
        {
            selectedWidget = null;
            selectedWidgetButton = null;
        }

        private void ShowSelectedWidget(int buttonIndex)
        {
            Control targetButton = widgetGrid.controls[buttonIndex];
            Control targetHUDPanel = hudGrid.controls[buttonIndex];

            widgetList.controls.Remove(selectedWidgetButton);
            gridWrap.AddChild(selectedWidgetButton);
            selectedWidgetButton.IgnoreLayout = true;
            selectedWidgetButton.PositionOffsetX = targetButton.GetRectTransform().anchoredPosition.x;
            selectedWidgetButton.PositionOffsetY = -targetButton.GetRectTransform().anchoredPosition.y;
            selectedWidgetButton.Value = false;

            selectedWidget.IsVisible = true;
            selectedWidget.PositionOffsetX = targetHUDPanel.GetRectTransform().anchoredPosition.x;
            selectedWidget.PositionOffsetY = -targetHUDPanel.GetRectTransform().anchoredPosition.y;

            selectedWidget = null;
            selectedWidgetButton = null;
        }

        private void HideWidget(Widget widget, SwitchButton button)
        {
            widget.IsVisible = false;
            widget.PositionOffsetX = 0;
            widget.PositionOffsetY = 0;

            button.IgnoreLayout = false;
            gridWrap.controls.Remove(button);
            widgetList.AddChild(button);
            button.PositionOffsetX = 0;
            button.PositionOffsetY = 0;
            button.Value = false;

            Canvas.ForceUpdateCanvases();
        }

        private void UpdateProvider_OnLateUpdate(object sender, EventArgs e)
        {
            if (hudWindow != null)
                hudWindow.Update();
        }
    }
}
