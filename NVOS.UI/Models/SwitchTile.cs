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
    public class SwitchTile : Control
    {
        private SwitchButton button;

        private Color deactivatedColor;
        private Color activatedColor;
        private Color highlightColor;
        private Color pressedColor;
        private Color textColor;

        private string text;
        private bool isOn;

        public event EventHandler<SwitchTileValueChangedEventArgs> OnValueChanged;

        public Color DeactivatedColor
        {
            get
            {
                return deactivatedColor;
            }
            set
            {
                deactivatedColor = value;
                button.DeactivatedColor = value;
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
                button.ActivatedColor = value;
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
                button.HighlightColor = value;
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
                button.PressedColor = value;
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
                button.Label.TextColor = value;
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
                button.Label.Text = value;
            }
        }

        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                isOn = value;
                button.IsOn = value;
            }
        }

        public SwitchTile() : this("Tile") { }

        public SwitchTile(string name) : base(name)
        {
            button = new SwitchButton();
            button.SizeScaleX = 1f;
            button.SizeScaleY = 1f;
            AddChild(button);

            PointerHandler pointerHandler = root.AddComponent<PointerHandler>();
            pointerHandler.PointerEnter += PointerHandler_PointerEnter;
            pointerHandler.PointerExit += PointerHandler_PointerExit;

            button.Label.Text = name;
            text = name;
            GameObject.Destroy(button.GetRootObject().GetComponent<HorizontalLayoutGroup>());
            button.Label.SizeScaleX = 0.9f;
            button.Label.SizeScaleY = 0.9f;
            button.Label.PositionScaleX = 0.05f;
            button.Label.PositionScaleY = 0.05f;
            button.Label.TextColor = Color.white;
            button.Label.FontSize = 0.5f;
            textColor = Color.white;
            button.Label.IsVisible = false;

            button.DeactivatedColor = Color.black;
            deactivatedColor = Color.black;
            button.ActivatedColor = new Color32(50, 50, 50, 255);
            activatedColor = new Color32(50, 50, 50, 255);
            button.HighlightColor = Color.gray;
            highlightColor = Color.gray;
            button.PressedColor = Color.white;
            pressedColor = Color.white;

            button.OnValueChanged += Button_OnValueChanged;
        }

        private void PointerHandler_PointerEnter(object sender, System.EventArgs e)
        {
            button.Label.IsVisible = true;
        }

        private void PointerHandler_PointerExit(object sender, System.EventArgs e)
        {
            button.Label.IsVisible = false;
        }

        private void Button_OnValueChanged(object sender, SwitchButtonValueChangedEventArgs e)
        {
            OnValueChanged?.Invoke(this, new SwitchTileValueChangedEventArgs(this, e.IsOn));
        }
    }
}
