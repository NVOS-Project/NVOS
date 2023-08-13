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

        private string deactivatedText;
        private string activatedText;
        private Color deactivatedTextColor;
        private Color activatedTextColor;

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

        public string DeactivatedText
        {
            get
            {
                return deactivatedText;
            }
            set
            {
                deactivatedText = value;
                button.DeactivatedText = value;
            }
        }

        public string ActivatedText
        {
            get
            {
                return activatedText;
            }
            set
            {
                activatedText = value;
                button.ActivatedText = value;
            }
        }

        public Color DeactivatedTextColor
        {
            get
            {
                return deactivatedTextColor;
            }
            set
            {
                deactivatedTextColor = value;
                button.DeactivatedTextColor = value;
            }
        }

        public Color ActivatedTextColor
        {
            get
            {
                return activatedTextColor;
            }
            set
            {
                activatedTextColor = value;
                button.ActivatedTextColor = value;
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

        public string Text
        {
            get
            {
                return button.Label.Text;
            }
            set
            {
                ActivatedText = value;
                DeactivatedText = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return button.Label.TextColor;
            }
            set
            {
                ActivatedTextColor = value;
                DeactivatedTextColor = value;
            }
        }

        public float FontSize
        {
            get
            {
                return button.Label.FontSize;
            }
            set
            {
                button.Label.FontSize = value;
            }
        }

        public SwitchTile() : this("Tile") { }

        public SwitchTile(string name) : base(name)
        {
            button = new SwitchButton(name);
            button.SizeScaleX = 1f;
            button.SizeScaleY = 1f;
            AddChild(button);

            PointerHandler pointerHandler = root.AddComponent<PointerHandler>();
            pointerHandler.PointerEnter += PointerHandler_PointerEnter;
            pointerHandler.PointerExit += PointerHandler_PointerExit;

            GameObject.Destroy(button.GetRootObject().GetComponent<HorizontalLayoutGroup>());
            button.Label.SizeScaleX = 0.9f;
            button.Label.SizeScaleY = 0.9f;
            button.Label.PositionScaleX = 0.05f;
            button.Label.PositionScaleY = 0.05f;
            button.Label.IsVisible = false;

            deactivatedColor = button.DeactivatedColor;
            activatedColor = button.ActivatedColor;
            highlightColor = button.HighlightColor;
            pressedColor = button.PressedColor;

            deactivatedText = name;
            activatedText = name;
            deactivatedTextColor = button.DeactivatedTextColor;
            activatedTextColor = button.ActivatedTextColor;

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
