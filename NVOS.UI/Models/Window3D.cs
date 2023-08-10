using NVOS.UI.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace NVOS.UI.Models
{
    public class Window3D : Window
    {
        protected Canvas canvas;
        private HorizontalLayoutPanel titleBar;
        private Label titleLabel;
        private Button minimizeButton;
        private Button closeButton;
        private SwitchButton lockButton;
        private Outline outline;
        private BoxCollider collider;

        private XRGrabInteractable grab;

        private bool showControls;
        private string title;
        private float outlineThickness;
        private bool renderOutline;

        private float width;
        private float height;

        private bool isLocked;

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value);
            }
        }

        public float OutlineThickness
        {
            get
            {
                return outlineThickness;
            }
            set
            {
                outline.effectDistance = new Vector2(value, value);
                outlineThickness = value;
            }
        }

        public bool RenderOutline
        {
            get
            {
                return renderOutline;
            }
            set
            {
                if (value)
                {
                    outline.effectColor = Color.clear;
                }
                else
                {
                    outline.effectColor = Color.black;
                }
                renderOutline = value;
            }
        }

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
                lockButton.IsVisible = value;
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

        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
        }

        public Window3D(string name, float width, float height) : base(name)
        {
            canvas = root.AddComponent<Canvas>();
            root.AddComponent<GraphicRaycaster>();
            root.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            rectTransform.sizeDelta = new Vector2(width, height);
            this.width = width;
            this.height = height;

            titleBar = new HorizontalLayoutPanel("TitleBar");
            showControls = true;
            titleBar.GetRootObject().transform.SetParent(root.transform, false);
            titleBar.BackgroundColor = Color.black;
            titleBar.PreferredHeight = height * 0.1f;

            titleLabel = new Label("TitleLabel");
            titleBar.AddChild(titleLabel);
            titleLabel.PreferredWidth = width * 0.7f;
            titleLabel.Text = name;
            title = name;
            titleLabel.TextColor = Color.white;
            titleLabel.FontSize = titleBar.PreferredHeight / 2;

            collider = root.AddComponent<BoxCollider>();
            float colliderX = (width - titleLabel.PreferredWidth) / -2;
            float colliderY = (height - titleBar.PreferredHeight) / -2;
            float colliderWidth = titleLabel.PreferredWidth;
            float colliderHeight = titleLabel.PreferredHeight;
            collider.center = new Vector3(colliderX, colliderY, 0);
            collider.size = new Vector3(colliderWidth, colliderHeight, 0.1f);

            Rigidbody rigidbody = root.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            grab = root.AddComponent<XRGrabInteractable>();
            grab.throwOnDetach = false;
            grab.useDynamicAttach = true;

            lockButton = new SwitchButton("Lock");
            titleBar.AddChild(lockButton);
            lockButton.DeactivatedColor = Color.black;
            lockButton.ActivatedColor = new Color32(50, 50, 50, 255);
            lockButton.HighlightColor = Color.gray;
            lockButton.PreferredWidth = width * 0.1f;
            lockButton.Label.Text = "L";
            lockButton.Label.TextColor = Color.white;
            lockButton.Label.FontSize = titleBar.PreferredHeight / 2;

            minimizeButton = new Button("Minimize");
            titleBar.AddChild(minimizeButton);
            minimizeButton.BackgroundColor = Color.black;
            minimizeButton.HighlightColor = Color.gray;
            minimizeButton.PreferredWidth = width * 0.1f;
            minimizeButton.Label.Text = "-";
            minimizeButton.Label.TextColor = Color.white;
            minimizeButton.Label.FontSize = titleBar.PreferredHeight / 2;

            closeButton = new Button("Close");
            titleBar.AddChild(closeButton);
            closeButton.BackgroundColor = Color.black;
            closeButton.HighlightColor = Color.gray;
            closeButton.PreferredWidth = width * 0.1f;
            closeButton.Label.Text = "X";
            closeButton.Label.TextColor = Color.white;
            closeButton.Label.FontSize = titleBar.PreferredHeight / 2;

            outline = root.AddComponent<Outline>();
            outline.effectDistance = new Vector2(0.01f, 0.01f);
            outlineThickness = 0.01f;
            outline.effectColor = Color.black;
            renderOutline = true;

            content.PreferredHeight = width - titleBar.PreferredHeight;

            minimizeButton.OnClick += MinimizeButton_OnClick;
            closeButton.OnClick += CloseButton_OnClick;
            lockButton.OnValueChanged += LockButton_OnValueChanged;
        }

        private void LockButton_OnValueChanged(object sender, SwitchButtonValueChangedEventArgs e)
        {
            isLocked = e.IsOn;
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

            float colliderX = (width - titleLabel.PreferredWidth) / -2;
            float colliderY = (height - titleBar.PreferredHeight) / -2;
            float colliderSize = titleLabel.PreferredWidth;
            collider.center = new Vector3(colliderX, colliderY, 0);
            collider.size = new Vector3(colliderSize, 0.1f, 0.1f);
        }
    }
}