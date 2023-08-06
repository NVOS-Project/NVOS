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

        private Vector2 cellSize;
        private Vector2 spacing;
        private Corner startCorner;
        private Axis startAxis;
        private Constraint constraint;
        private int constraintCount;

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
