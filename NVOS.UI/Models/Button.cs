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
    public class Button : Control
    {
        private Color backgroundColor;
        private UnityEngine.UI.Button button;
        private Image backgroundImage;

        public event EventHandler<System.EventArgs> OnClick;

        public Label Label { get; }
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

        public Button() : base("Button")
        {
            Label = new Label("ButtonLabel");
            AddChild(Label);
            root.AddComponent<HorizontalLayoutGroup>();
            button = root.AddComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(HandleClick));
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.gray;

            SizeOffsetX = 0.4f;
            SizeOffsetY = 0.15f;
        }

        public Button(string name) : base(name)
        {
            Label = new Label($"{name}Label");
            AddChild(Label);
            root.AddComponent<HorizontalLayoutGroup>();
            button = root.AddComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(HandleClick));
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.gray;

            SizeOffsetX = 0.4f;
            SizeOffsetY = 0.15f;
        }

        private void HandleClick()
        {
            OnClick?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
