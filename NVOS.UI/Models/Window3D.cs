using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI.Models
{
    public class Window3D : Window
    {
        protected Canvas canvas;
        private HorizontalLayoutPanel titleBar;
        private Label titleLabel;
        private Button minimizeButton;
        private Button closeButton;

        public Window3D() : this("Window") { }

        public Window3D(string title) : base(title)
        {
            canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            rectTransform.sizeDelta = new Vector2(1.5f, 1f);

            titleBar = new HorizontalLayoutPanel("TitleBar");
            titleBar.GetRootObject().transform.SetParent(root.transform);
            titleBar.BackgroundColor = Color.black;
            titleBar.PreferredHeight = rectTransform.sizeDelta.y * 0.1f;

            titleLabel = new Label("TitleLabel");
            titleBar.AddChild(titleLabel);
            titleLabel.PreferredWidth = rectTransform.sizeDelta.x * 0.7f;
            titleLabel.Text = title;
            titleLabel.TextColor = Color.white;
            titleLabel.FontSize = 0.05f;

            minimizeButton = new Button("Minimize");
            titleBar.AddChild(minimizeButton);
            minimizeButton.BackgroundColor = Color.black;
            minimizeButton.PreferredWidth = rectTransform.sizeDelta.x * 0.15f;
            minimizeButton.Label.Text = "-";
            minimizeButton.Label.TextColor = Color.white;
            minimizeButton.Label.FontSize = 0.05f;

            closeButton = new Button("Close");
            titleBar.AddChild(closeButton);
            closeButton.BackgroundColor = Color.black;
            closeButton.PreferredWidth = rectTransform.sizeDelta.x * 0.15f;
            closeButton.Label.Text = "X";
            closeButton.Label.TextColor = Color.white;
            closeButton.Label.FontSize = 0.05f;

            content.PreferredHeight = rectTransform.sizeDelta.y * 0.9f;
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }
    }
}
