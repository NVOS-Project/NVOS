using NVOS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

namespace NVOS.UI
{
    public class GridLayoutPanel : Control
    {
        private GridLayoutGroup layoutGroup;
        private Image backgroundImage;
        private Color backgroundColor;

        private TextAnchor childAlignment;
        private Vector2 cellSize;
        private Vector2 spacing;
        private Corner startCorner;
        private Axis startAxis;
        private Constraint constraint;
        private int constraintCount;

        private int paddingTop = 0;
        private int paddingBottom = 0;
        private int paddingLeft = 0;
        private int paddingRight = 0;

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

        public Corner StartCorner
        {
            get
            {
                return startCorner;
            }
            set
            {
                startCorner = value;
                layoutGroup.startCorner = value;
            }
        }

        public Axis StartAxis
        {
            get
            {
                return startAxis;
            }
            set
            {
                startAxis = value;
                layoutGroup.startAxis = value;
            }
        }

        public Constraint Constraint
        {
            get
            {
                return constraint;
            }
            set
            {
                constraint = value;
                layoutGroup.constraint = value;
            }
        }

        public int ConstraintCount
        {
            get
            {
                return constraintCount;
            }
            set
            {
                constraintCount = value;
                layoutGroup.constraintCount = value;
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

        public Vector2 CellSize
        {
            get
            {
                return cellSize;
            }
            set
            {
                cellSize = value;
                layoutGroup.cellSize = value;
            }
        }

        public Vector2 Spacing
        {
            get
            {
                return spacing;
            }
            set
            {
                spacing = value;
                layoutGroup.spacing = value;
            }
        }

        public GridLayoutPanel() : this("LayoutPanel") { }

        public GridLayoutPanel(string name) : base(name)
        {
            layoutGroup = root.AddComponent<GridLayoutGroup>();
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
        }
    }
}
