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

        private bool showControls;
        private string title;

        public bool ShowControls
        {
            get
            {
                return showControls;
            }
            set
            {
                showControls = value;
                minimizeButton.IsVisible = value;
                closeButton.IsVisible = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                titleLabel.Text = value;
            }
        }

        public Window3D() : this("Window") { }

        public Window3D(string title) : base(title)
        {
            canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            rectTransform.sizeDelta = new Vector2(1.5f, 1f);

            titleBar = new HorizontalLayoutPanel("TitleBar");
            showControls = true;
            titleBar.GetRootObject().transform.SetParent(root.transform);
            titleBar.BackgroundColor = Color.black;
            titleBar.PreferredHeight = 0.1f;

            titleLabel = new Label("TitleLabel");
            titleBar.AddChild(titleLabel);
            titleLabel.PreferredWidth = rectTransform.sizeDelta.x * 0.7f;
            titleLabel.Text = title;
            this.title = title;
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

            content.PreferredHeight = rectTransform.sizeDelta.y - titleBar.PreferredHeight;

            minimizeButton.OnClick += MinimizeButton_OnClick;
            closeButton.OnClick += CloseButton_OnClick;
        }

        private void MinimizeButton_OnClick(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void CloseButton_OnClick(object sender, System.EventArgs e)
        {
            Close();
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }

        public override void Update()
        {
            base.Update();
            content.PreferredHeight = rectTransform.sizeDelta.y - titleBar.PreferredHeight;
        }
    }
}