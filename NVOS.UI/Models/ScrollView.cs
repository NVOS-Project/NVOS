using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using static UnityEngine.UI.ContentSizeFitter;

namespace NVOS.UI.Models
{
    public class ScrollView : Control 
    {
        private Image backgroundImage;
        private ScrollRect scrollRect;
        private Panel horizontalScrollbarPanel;
        private Panel verticalScrollbarPanel;

        private Color backgroundColor;
        private bool horizontalScroll;
        private bool verticalScroll;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                backgroundImage.color = value;
            }
        }

        public bool HorizontalScroll
        {
            get
            {
                return horizontalScroll;
            }
            set
            {
                horizontalScroll = value;
                scrollRect.horizontal = value;
            }
        }

        public bool VerticalScroll
        {
            get
            {
                return verticalScroll;
            }
            set
            {
                verticalScroll = value;
                scrollRect.vertical = value;
            }
        }

        public ScrollView() : this("ScrollView", new Panel()) { }

        public ScrollView(string name, Control content) : base(name)
        {
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.gray;
            scrollRect = root.AddComponent<ScrollRect>();
            SizeOffsetX = 30f;
            SizeOffsetY = 30f;

            Panel viewport = new Panel("Viewport");
            viewport.BackgroundColor = new Color(255, 255, 255, 1);
            viewport.GetRootObject().AddComponent<Mask>().showMaskGraphic = false;

            viewport.SizeScaleX = 1f;
            viewport.SizeScaleY = 1f;

            viewport.AddChild(content);
            AddChild(viewport);
            scrollRect.content = content.GetRootObject().GetComponent<RectTransform>();
            scrollRect.viewport = viewport.GetRootObject().GetComponent<RectTransform>();

            horizontalScrollbarPanel = new Panel("Horizontal Scrollbar");
            horizontalScrollbarPanel.BackgroundColor = Color.clear;
            AddChild(horizontalScrollbarPanel);
            horizontalScrollbarPanel.SizeScaleX = 1f;
            horizontalScrollbarPanel.SizeOffsetY = 0.3f;
            horizontalScrollbarPanel.PositionOffsetY = 29.7f;
            Scrollbar horizontalScrollbar = horizontalScrollbarPanel.GetRootObject().AddComponent<Scrollbar>();

            verticalScrollbarPanel = new Panel("Vertical Scrollbar");
            verticalScrollbarPanel.BackgroundColor = Color.clear;
            AddChild(verticalScrollbarPanel);
            verticalScrollbarPanel.SizeOffsetX = 0.3f;
            verticalScrollbarPanel.SizeScaleY = 1f;
            verticalScrollbarPanel.PositionOffsetX = 29.7f;
            Scrollbar verticalScrollbar = verticalScrollbarPanel.GetRootObject().AddComponent<Scrollbar>();
            verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;

            Panel horizontalArea = new Panel("Sliding Area");
            horizontalArea.BackgroundColor = Color.clear;
            horizontalArea.SizeScaleX = 1f;
            horizontalArea.SizeScaleY = 1f;
            horizontalScrollbarPanel.AddChild(horizontalArea);

            Panel verticalArea = new Panel("Sliding Area");
            verticalArea.BackgroundColor = Color.clear;
            verticalArea.SizeScaleX = 1f;
            verticalArea.SizeScaleY = 1f;
            verticalScrollbarPanel.AddChild(verticalArea);

            Panel horizontalHandle = new Panel("Handle");
            horizontalHandle.BackgroundColor = Color.black;
            horizontalHandle.SizeScaleX = 1f;
            horizontalHandle.SizeScaleY = 1f;
            horizontalArea.AddChild(horizontalHandle);
            horizontalScrollbar.interactable = false;
            horizontalScrollbar.handleRect = horizontalHandle.GetRootObject().GetComponent<RectTransform>();

            Panel verticalHandle = new Panel("Handle");
            verticalHandle.BackgroundColor = Color.black;
            verticalHandle.SizeScaleX = 1f;
            verticalHandle.SizeScaleY = 1f;
            verticalArea.AddChild(verticalHandle);
            verticalScrollbar.interactable = false;
            verticalScrollbar.handleRect = verticalHandle.GetRootObject().GetComponent<RectTransform>();

            scrollRect.horizontalScrollbar = horizontalScrollbar;
            scrollRect.verticalScrollbar = verticalScrollbar;
            scrollRect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
        }

        public override void Update()
        {
            base.Update();
            horizontalScrollbarPanel.PositionOffsetY = Height - horizontalScrollbarPanel.Height;
            verticalScrollbarPanel.PositionOffsetX = Width - verticalScrollbarPanel.Width;
        }
    }
}
