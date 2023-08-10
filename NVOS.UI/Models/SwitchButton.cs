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

        private bool isOn;

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
                if (!isOn)
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
                if (isOn)
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

        public bool IsOn
        {
            get
            {
                return isOn;
            }
            set
            {
                SetActivated(value);
            }
        }

        public Label Label { get; }

        public SwitchButton() : this("Switch Button") { }

        public SwitchButton(string name) : base(name)
        {
            button = new Button("Switch Button");
            button.SizeScaleX = 1f;
            button.SizeScaleY = 1f;
            AddChild(button);
            root.AddComponent<HorizontalLayoutGroup>();
            Label = button.Label;
            deactivatedColor = button.BackgroundColor;
            activatedColor = new Color32(50, 50, 50, 255);
            highlightColor = button.HighlightColor;
            pressedColor = button.PressedColor;
            isOn = false;

            button.OnClick += Button_OnClick;
        }

        private void Button_OnClick(object sender, System.EventArgs e)
        {
            SetActivated(!isOn);
            OnValueChanged?.Invoke(this, new SwitchButtonValueChangedEventArgs(this, isOn));
        }

        private void SetActivated(bool value)
        {
            isOn = value;
            if (value)
                button.BackgroundColor = activatedColor;
            else
                button.BackgroundColor = deactivatedColor;
        }
    }
}
