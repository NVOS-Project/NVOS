using NVOS.UI.Models.EventArgs;
using NVOS.UI.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class ToggleTile : Control
    {

        private UnityEngine.UI.Toggle toggle;
        private Panel deactivatedPanel;
        private Panel activatedPanel;
        private Label label;

        private Color deactivatedColor;
        private Color activatedColor;
        private Color highlightColor;
        private Color pressedColor;
        private Color textColor;

        private string text;

        public event EventHandler<ToggleTileValueChangedEventArgs> OnValueChanged;

        public Color DeactivatedColor
        {
            get
            {
                return deactivatedColor;
            }
            set
            {
                deactivatedColor = value;
                ColorBlock colorBlock = toggle.colors;
                colorBlock.normalColor = value;
                toggle.colors = colorBlock;
            }
        }

        public Color ActivatedColor
        {
            get
            {
                return activatedColor;
            }
            set
            {
                activatedColor = value;
                activatedPanel.BackgroundColor = value;
            }
        }

        public Color HighlightColor
        {
            get
            {
                return highlightColor;
            }
            set
            {
                highlightColor = value;
                ColorBlock colorBlock = toggle.colors;
                colorBlock.highlightedColor = value;
                toggle.colors = colorBlock;
            }
        }

        public Color PressedColor
        {
            get
            {
                return pressedColor;
            }
            set
            {
                pressedColor = value;
                ColorBlock colorBlock = toggle.colors;
                colorBlock.pressedColor = value;
                toggle.colors = colorBlock;
            }
        }

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                label.TextColor = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                label.Text = value;
            }
        }

        public ToggleTile() : this("Tile") { }

        public ToggleTile(string name) : base(name)
        {
            Image image = root.AddComponent<Image>();
            image.color = Color.white;

            PointerHandler pointerHandler = root.AddComponent<PointerHandler>();
            pointerHandler.PointerEnter += PointerHandler_PointerEnter;
            pointerHandler.PointerExit += PointerHandler_PointerExit;

            deactivatedPanel = new Panel("Deactivated");
            AddChild(deactivatedPanel);
            deactivatedPanel.BackgroundColor = Color.white;

            deactivatedPanel.SizeScaleX = 1f;
            deactivatedPanel.SizeScaleY = 1f;

            activatedPanel = new Panel("Activated");
            AddChild(activatedPanel);
            activatedPanel.BackgroundColor = Color.green;
            activatedColor = Color.green;

            label = new Label(name);
            activatedPanel.AddChild(label);
            label.SizeOffsetX = 0f;
            label.SizeOffsetY = 0f;
            label.SizeScaleX = 0.9f;
            label.SizeScaleY = 0.9f;
            label.PositionScaleX = 0.05f;
            label.PositionScaleY = 0.05f;
            label.TextColor = Color.white;
            textColor = Color.white;
            label.FontSize = 2f;
            label.IsVisible = false;

            activatedPanel.SizeScaleX = 1f;
            activatedPanel.SizeScaleY = 1f;

            toggle = root.AddComponent<UnityEngine.UI.Toggle>();
            toggle.image = deactivatedPanel.GetRootObject().GetComponent<Image>();
            toggle.graphic = activatedPanel.GetRootObject().GetComponent<Image>();
            toggle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>(HandleClick));

            Navigation navigation = toggle.navigation;
            navigation.mode = Navigation.Mode.None;
            toggle.navigation = navigation;

            ColorBlock colorBlock = toggle.colors;
            colorBlock.highlightedColor = Color.gray;
            colorBlock.pressedColor = Color.white;
            colorBlock.normalColor = Color.red;
            toggle.colors = colorBlock;
            highlightColor = Color.gray;
            pressedColor = Color.white;
            deactivatedColor = Color.red;
        }

        private void PointerHandler_PointerEnter(object sender, System.EventArgs e)
        {
            label.IsVisible = true;
        }

        private void PointerHandler_PointerExit(object sender, System.EventArgs e)
        {
            label.IsVisible = false;
        }

        private void HandleClick(bool isActivated)
        {
            OnValueChanged?.Invoke(this, new ToggleTileValueChangedEventArgs(this, isActivated));
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
