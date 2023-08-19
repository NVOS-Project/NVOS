using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class HorizontalLayoutPanel : Control
    {
        private HorizontalLayoutGroup layoutGroup;
        private Image backgroundImage;
        private Color backgroundColor;
        private TextAnchor childAlignment;

        private float spacing;
        private bool reverseArrangement;
        private bool controlChildWidth;
        private bool controlChildHeight;
        private bool useChildWidth;
        private bool useChildHeight;
        private bool forceChildWidth;
        private bool forceChildHeight;

        private int paddingTop;
        private int paddingBottom;
        private int paddingLeft;
        private int paddingRight;

        public int PaddingTop
        {
            get
            {
                return paddingTop;
            }
            set
            {
                layoutGroup.padding.top = value;
                paddingTop = value;
            }
        }

        public int PaddingBottom
        {
            get
            {
                return paddingBottom;
            }
            set
            {
                layoutGroup.padding.bottom = value;
                paddingBottom = value;
            }
        }

        public int PaddingLeft
        {
            get
            {
                return paddingLeft;
            }
            set
            {
                layoutGroup.padding.left = value;
                paddingLeft = value;
            }
        }

        public int PaddingRight
        {
            get
            {
                return paddingRight;
            }
            set
            {
                layoutGroup.padding.right = value;
                paddingRight = value;
            }
        }

        public TextAnchor ChildAlignment
        {
            get
            {
                return childAlignment;
            }
            set
            {
                layoutGroup.childAlignment = value;
                childAlignment = value;
            }
        }

        public float Spacing
        {
            get
            {
                return spacing;
            }
            set
            {
                layoutGroup.spacing = value;
                spacing = value;
            }
        }

        public bool ReverseArrangement
        {
            get
            {
                return reverseArrangement;
            }
            set
            {
                layoutGroup.reverseArrangement = value;
                reverseArrangement = value;
            }
        }

        public bool ControlChildWidth
        {
            get
            {
                return controlChildWidth;
            }
            set
            {
                layoutGroup.childControlWidth = value;
                controlChildWidth = value;
            }
        }

        public bool ControlChildHeight
        {
            get
            {
                return controlChildHeight;
            }
            set
            {
                layoutGroup.childControlHeight = value;
                controlChildHeight = value;
            }
        }

        public bool UseChildWidth
        {
            get
            {
                return useChildWidth;
            }
            set
            {
                layoutGroup.childScaleWidth = value;
                useChildWidth = value;
            }
        }

        public bool UseChildHeight
        {
            get
            {
                return useChildHeight;
            }
            set
            {
                layoutGroup.childScaleHeight = value;
                useChildHeight = value;
            }
        }

        public bool ForceChildWidth
        {
            get
            {
                return forceChildWidth;
            }
            set
            {
                layoutGroup.childForceExpandWidth = value;
                forceChildWidth = value;
            }
        }

        public bool ForceChildHeight
        {
            get
            {
                return forceChildHeight;
            }
            set
            {
                layoutGroup.childForceExpandHeight = value;
                forceChildHeight = value;
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
                backgroundImage.color = value;
                backgroundColor = value;
            }
        }

        public HorizontalLayoutPanel() : this("LayoutPanel") { }

        public HorizontalLayoutPanel(string name) : base(name)
        {
            layoutGroup = root.AddComponent<HorizontalLayoutGroup>();
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
        }
    }
}