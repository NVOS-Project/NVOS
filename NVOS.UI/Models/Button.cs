﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Button : Control
    {
        private UnityEngine.UI.Button button;
        private Image buttonImage;

        private Color highlightColor;
        private Color backgroundColor;

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

        public Button() : this("Button") { }

        public Button(string name) : base(name)
        {
            Label = new Label($"{name}Label");
            AddChild(Label);
            Label.FontSize = 2f;
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
            colorBlock.normalColor = Color.gray;
            colorBlock.highlightedColor = Color.white;
            button.colors = colorBlock;
            
            backgroundColor = Color.gray;
            highlightColor = Color.white;

            SizeOffsetX = 15f;
            SizeOffsetY = 5f;
        }

        private void HandleClick()
        {
            OnClick?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
