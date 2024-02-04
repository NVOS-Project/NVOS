using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Widget : Control
    {
        private Image backgroundImage;
        private Color backgroundColor;
        private Color previewColor;

        private int tileWidth;
        private int tileHeight;

        private bool isSelected;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundImage.color = value;
                backgroundColor = value;
            }
        }

        public Color PreviewColor
        {
            get
            {
                return previewColor;
            }
            set
            {
                previewColor = value;
            }
        }

        public int TileWidth 
        {
            get
            {
                return tileWidth;
            }
        }

        public int TileHeight
        {
            get
            {
                return tileHeight;
            }
        }

        public Widget() : this("Widget", 1, 1) { }

        public Widget(string name, int width, int height) : base(name)
        {
            isSelected = false;
            tileWidth = width;
            tileHeight = height;
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
            previewColor = Color.white;
        }
    }
}
