using System;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Button : Control
    {
        private UnityEngine.UI.Button button;
        private Image buttonImage;

        private Color highlightColor;
        private Color pressedColor;
        private Color backgroundColor;

        private bool interactable;

        public event EventHandler<System.EventArgs> OnClick;

        public Label Label { get; }
        public Color HighlightColor
        {
            get
            {
                return highlightColor;
            }
            set
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.highlightedColor = value;
                button.colors = colorBlock;
                highlightColor = value;
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
                ColorBlock colorBlock = button.colors;
                colorBlock.pressedColor = value;
                button.colors = colorBlock;
                pressedColor = value;
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                ColorBlock colorBlock = button.colors;
                colorBlock.normalColor = value;
                button.colors = colorBlock;
                backgroundColor = value;
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
                button.interactable = value;
            }
        }

        public Button() : this("Button") { }

        public Button(string name) : base(name)
        {
            Label = new Label(name);
            AddChild(Label);
            Label.FontSize = 2f;
            Label.TextColor = Color.white;

            Label.SizeScaleX = 1f;
            Label.SizeScaleY = 1f;

            root.AddComponent<HorizontalLayoutGroup>();
            button = root.AddComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(HandleClick));
            buttonImage = root.AddComponent<Image>();
            buttonImage.color = Color.white;
            button.targetGraphic = buttonImage;

            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.None;
            button.navigation = navigation;

            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = Color.black;
            colorBlock.highlightedColor = new Color32(70, 70, 70, 255);
            colorBlock.pressedColor = new Color32(170, 170, 170, 255);
            button.colors = colorBlock;

            backgroundColor = Color.black;
            highlightColor = new Color32(70, 70, 70, 255);
            pressedColor = new Color32(170, 170, 170, 255);

            interactable = true;
        }

        private void HandleClick()
        {
            OnClick?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
