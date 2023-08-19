using System;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class ButtonTile : Control
    {
        private Button button;

        private Color backgroundColor;
        private Color highlightColor;
        private Color pressedColor;
        private Color textColor;

        private bool interactable;

        private string text;

        public event EventHandler<System.EventArgs> OnClick;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
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

        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                button.Interactable = value;
            }
        }

        public ButtonTile() : this("Tile") { }

        public ButtonTile(string name) : base(name)
        {
            button = new Button();
            button.SizeScaleX = 1f;
            button.SizeScaleY = 1f;
            AddChild(button);

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

            backgroundColor = button.BackgroundColor;
            highlightColor = button.HighlightColor;
            pressedColor = button.PressedColor;

            button.OnClick += Button_OnClick;
            interactable = true;
        }

        private void Button_OnClick(object sender, System.EventArgs e)
        {
            OnClick?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
