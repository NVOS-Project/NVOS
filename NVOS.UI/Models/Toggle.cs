using NVOS.UI.Models.EventArgs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Toggle : Control
    {
        private Panel check;
        private Panel uncheck;
        private Image backgroundImage;
        private UnityEngine.UI.Toggle toggle;

        private Color backgroundColor;
        private Color checkColor;
        private Color uncheckColor;
        private Color highlightColor;
        private Color pressedColor;

        private bool interactable;

        public event EventHandler<ToggleValueChangedEventArgs> OnValueChanged;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                backgroundImage.color = value;
            }
        }

        public Color CheckColor
        {
            get
            {
                return checkColor;
            }
            set
            {
                checkColor = value;
                check.BackgroundColor = value;
            }
        }

        public Color UncheckColor
        {
            get
            {
                return uncheckColor;
            }
            set
            {
                uncheckColor = value;
                ColorBlock colorBlock = toggle.colors;
                colorBlock.normalColor = value;
                toggle.colors = colorBlock;
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

        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                toggle.interactable = value;
            }
        }

        public Toggle() : this("Toggle") { }

        public Toggle(string name) : base(name)
        {
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.gray;
            backgroundColor = Color.gray;

            uncheck = new Panel("Uncheck");
            AddChild(uncheck);
            uncheck.BackgroundColor = Color.white;

            uncheck.SizeScaleX = 0.7f;
            uncheck.SizeScaleY = 0.7f;
            uncheck.PositionScaleX = 0.15f;
            uncheck.PositionScaleY = 0.15f;

            check = new Panel("Check");
            AddChild(check);
            check.BackgroundColor = Color.black;
            checkColor = Color.black;

            check.SizeScaleX = 0.7f;
            check.SizeScaleY = 0.7f;
            check.PositionScaleX = 0.15f;
            check.PositionScaleY = 0.15f;

            toggle = root.AddComponent<UnityEngine.UI.Toggle>();
            toggle.image = uncheck.GetRootObject().GetComponent<Image>();
            toggle.graphic = check.GetRootObject().GetComponent<Image>();
            toggle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>(HandleClick));

            Navigation navigation = toggle.navigation;
            navigation.mode = Navigation.Mode.None;
            toggle.navigation = navigation;

            ColorBlock colorBlock = toggle.colors;
            colorBlock.highlightedColor = Color.gray;
            colorBlock.pressedColor = Color.white;
            colorBlock.normalColor = Color.white;
            toggle.colors = colorBlock;
            highlightColor = Color.gray;
            pressedColor = Color.white;
            uncheck.BackgroundColor = Color.white;

            interactable = true;
        }

        private void HandleClick(bool isChecked)
        {
            OnValueChanged?.Invoke(this, new ToggleValueChangedEventArgs(this, isChecked));
        }
    }
}
