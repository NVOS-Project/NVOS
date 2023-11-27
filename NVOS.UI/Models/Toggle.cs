using NVOS.UI.Models.EventArgs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Toggle : Control
    {
        private Panel check;
        private Panel backgroundPanel;
        private Panel mainPanel;
        private UnityEngine.UI.Toggle toggle;

        private Color backgroundColor;
        private Color checkColor;
        private Color highlightColor;
        private Color pressedColor;

        private bool enabled;

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
                backgroundPanel.BackgroundColor = value;
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

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                toggle.interactable = value;
            }
        }

        public Label Label { get; }

        public Toggle() : this("Toggle") { }

        public Toggle(string name) : base(name)
        {
            HorizontalLayoutGroup horizontalGroup = root.AddComponent<HorizontalLayoutGroup>();
            horizontalGroup.childForceExpandWidth = false;

            mainPanel = new Panel("Main Panel");
            mainPanel.SizeScaleX = 1f;
            mainPanel.SizeScaleY = 1f;
            mainPanel.BackgroundColor = Color.clear;
            AddChild(mainPanel);

            backgroundPanel = new Panel("Toggle Panel");
            mainPanel.AddChild(backgroundPanel);
            backgroundPanel.SizeScaleX = 0.5f;
            backgroundPanel.SizeScaleY = 0.5f;
            backgroundPanel.PositionScaleX = 0.25f;
            backgroundPanel.PositionScaleY = 0.25f;

            backgroundPanel.BackgroundColor = Color.gray;
            backgroundColor = Color.gray;

            check = new Panel("Check");
            backgroundPanel.AddChild(check);
            check.BackgroundColor = Color.black;
            checkColor = Color.black;

            check.SizeScaleX = 0.7f;
            check.SizeScaleY = 0.7f;
            check.PositionScaleX = 0.15f;
            check.PositionScaleY = 0.15f;

            Label = new Label("Label");
            AddChild(Label);
            Label.Text = name;
            Label.TextAlignment = TMPro.TextAlignmentOptions.Left;

            toggle = root.AddComponent<UnityEngine.UI.Toggle>();
            toggle.graphic = check.GetRootObject().GetComponent<Image>();
            toggle.image = backgroundPanel.GetRootObject().GetComponent<Image>();
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

            enabled = true;
        }

        public override void Update()
        {
            base.Update();
            mainPanel.PreferredWidth = Height;
            Label.PreferredWidth = Width - mainPanel.PreferredWidth;
        }

        private void HandleClick(bool isChecked)
        {
            OnValueChanged?.Invoke(this, new ToggleValueChangedEventArgs(this, isChecked));
        }
    }
}
