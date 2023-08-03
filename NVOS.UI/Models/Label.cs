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

        public Label() : this("Label") { }

        public Label(string name) : base(name)
        {
            textComponent = root.AddComponent<TextMeshProUGUI>();
            textComponent.text = name;
            text = name;
            textComponent.fontSize = 0.05f;
            fontSize = 0.05f;
            textComponent.alignment = TextAlignmentOptions.Center;
            textAlignment = TextAlignmentOptions.Center;
            textComponent.color = Color.black;
            textColor = Color.black;

            SizeOffsetX = 0.4f;
            SizeOffsetY = 0.15f;
        }
    }
}
