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
    public class ButtonTile : Control
    {
        private Button button;

        private Color backgroundColor;
        private Color highlightColor;
        private Color pressedColor;
        private Color textColor;

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

        public ButtonTile() : this("Tile") { }

        public ButtonTile(string name) : base(name)
        {
            button = new Button();
            button.SizeOffsetX = 0f;
            button.SizeOffsetY = 0f;
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

            button.BackgroundColor = Color.black;
            BackgroundColor = Color.black;
            button.HighlightColor = new Color(10f, 10f, 10f);
            HighlightColor = Color.gray;
            button.PressedColor = Color.white;
            PressedColor = Color.white;

            button.OnClick += Button_OnClick;
        }

        private void PointerHandler_PointerEnter(object sender, System.EventArgs e)
        {
            button.Label.IsVisible = true;
        }

        private void PointerHandler_PointerExit(object sender, System.EventArgs e)
        {
            button.Label.IsVisible = false;
        }

        private void Button_OnClick(object sender, System.EventArgs e)
        {
            OnClick?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
