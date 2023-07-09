using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class HorizontalLayoutPanel : Control
    {
        private HorizontalLayoutGroup layoutGroup;
        private Image backgroundImage;
        private Color backgroundColor;

        private float spacing;
        private bool reverseArrangement;
        private bool controlChildWidth;
        private bool controlChildHeight;
        private bool useChildWidth;
        private bool useChildHeight;
        private bool forceChildWidth;
        private bool forceChildHeight;

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

        public HorizontalLayoutPanel() : base("LayoutPanel")
        {
            layoutGroup = root.AddComponent<HorizontalLayoutGroup>();
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
        }

        public HorizontalLayoutPanel(string name) : base(name)
        {
            layoutGroup = root.AddComponent<HorizontalLayoutGroup>();
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
        }
    }
}