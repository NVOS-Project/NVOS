using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Label : Control
    {
        private string text;
        private float fontSize;
        private TextAlignmentOptions textAlignment;
        private Color textColor;
        private TextOverflowModes overflow;
        private Vector4 margin;

        private TextMeshProUGUI textComponent;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                textComponent.text = value;
                text = value;
            }
        }
        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                textComponent.fontSize = value;
                fontSize = value;
            }
        }
        public TextAlignmentOptions TextAlignment
        {
            get
            {
                return textAlignment;
            }
            set
            {
                textComponent.alignment = value;
                textAlignment = value;
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
                textComponent.color = value;
                textColor = value;
            }
        }

        public TextOverflowModes Overflow
        {
            get
            {
                return overflow;
            }
            set
            {
                textComponent.overflowMode = value;
                overflow = value;
            }
        }

        public Vector4 Margin
        {
            get
            {
                return margin;
            }
            set
            {
                textComponent.margin = value;
                margin = value;
            }
        }

        public Label() : this("Label") { }

        public Label(string name) : base(name)
        {
            SizeScaleX = 1f;
            SizeScaleY = 1f;
            textComponent = root.AddComponent<TextMeshProUGUI>();
            textComponent.text = name;
            text = name;
            textComponent.fontSize = 1f;
            fontSize = 1f;
            textComponent.alignment = TextAlignmentOptions.Center;
            textAlignment = TextAlignmentOptions.Center;
            textComponent.color = Color.black;
            textColor = Color.black;
            textComponent.overflowMode = TextOverflowModes.Ellipsis;
            textComponent.margin = new Vector4(0, 0, 0, 0);

        }
    }
}
