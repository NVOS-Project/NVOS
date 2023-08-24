using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NVOS.UI.Models.EventArgs;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class SwitchButton : Control
    {
        private Button button;

        private Color deactivatedColor;
        private Color activatedColor;
        private Color highlightColor;
        private Color pressedColor;

        private string deactivatedText;
        private string activatedText;
        private Color deactivatedTextColor;
        private Color activatedTextColor;

        private bool value;
        private bool enabled;

        public event EventHandler<SwitchButtonValueChangedEventArgs> OnValueChanged;

        public Color DeactivatedColor
        {
            get
            {
                return deactivatedColor;
            }
            set
            {
                deactivatedColor = value;
                if (!this.value)
                    button.BackgroundColor = value;
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
                if (this.value)
                    button.BackgroundColor = value;
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
                if (!this.value)
                    button.Label.Text = value;
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
                if (this.value)
                    button.Label.Text = value;
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
                if (!this.value)
                    button.Label.TextColor = value;
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
                if (this.value)
                    button.Label.TextColor = value;
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

        public bool Value
        {
            get
            {
                return value;
            }
            set
            {
                SetActivated(value);
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                button.Enabled = value;
            }
        }

        public Label Label { get; }

        public SwitchButton() : this("SwitchButton") { }

        public SwitchButton(string name) : base(name)
        {
            button = new Button(name);
            Label = button.Label;
            button.SizeScaleX = 1f;
            button.SizeScaleY = 1f;
            AddChild(button);
            root.AddComponent<HorizontalLayoutGroup>();

            deactivatedColor = button.BackgroundColor;
            activatedColor = new Color32(100, 100, 100, 255);
            highlightColor = button.HighlightColor;
            pressedColor = button.PressedColor;

            deactivatedText = button.Label.Text;
            activatedText = button.Label.Text;
            deactivatedTextColor = button.Label.TextColor;
            activatedTextColor = button.Label.TextColor;

            value = false;

            button.OnClick += Button_OnClick;
            enabled = true;
        }


        private void Button_OnClick(object sender, System.EventArgs e)
        {
            SetActivated(!value);
            OnValueChanged?.Invoke(this, new SwitchButtonValueChangedEventArgs(this, value));
        }

        private void SetActivated(bool value)
        {
            this.value = value;
            if (value)
            {
                button.BackgroundColor = activatedColor;
                button.Label.Text = activatedText;
                button.Label.TextColor = activatedTextColor;
            }
            else
            {
                button.BackgroundColor = deactivatedColor;
                button.Label.Text = deactivatedText;
                button.Label.TextColor = deactivatedTextColor;
            }
        }
    }
}
