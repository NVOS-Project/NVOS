using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class HorizontalSlider : Control
    {
        private Slider slider;
        private Panel backgroundPanel;
        private Panel fillArea;
        private Panel fill;
        private Panel handleArea;
        private Panel handle;

        private Color backgroundColor;
        private Color fillColor;
        private Color handleColor;
        private Slider.Direction direction;

        private float value;
        private float minValue;
        private float maxValue;

        private bool wholeNumbers;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                backgroundPanel.BackgroundColor = value;
            }
        }

        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
                fill.BackgroundColor = value;
            }
        }

        public Color HandleColor
        {
            get
            {
                return handleColor;
            }
            set
            {
                handleColor = value;
                handle.BackgroundColor = value;
            }
        }

        public Slider.Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if (value != Slider.Direction.LeftToRight && value != Slider.Direction.RightToLeft)
                    throw new Exception("Horizontal slider only accepts a LeftToRight/RightToLeft direction!");

                    direction = value;
                slider.direction = value;
            }
        }

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                slider.value = value;
            }
        }

        public float MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                slider.minValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                slider.maxValue = value;
            }
        }

        public bool WholeNumbers
        {
            get
            {
                return wholeNumbers;
            }
            set
            {
                wholeNumbers = value;
                slider.wholeNumbers = value;
            }
        }

        public HorizontalSlider() : this("Slider") { }

        public HorizontalSlider(string name) : base(name)
        {
            slider = root.AddComponent<Slider>();
            slider.direction = Slider.Direction.LeftToRight;
            SizeOffsetX = 30f;
            SizeOffsetY = 2f;

            backgroundPanel = new Panel("Background");
            AddChild(backgroundPanel);
            backgroundPanel.BackgroundColor = Color.gray;
            backgroundColor = Color.gray;
            backgroundPanel.SizeScaleX = 1f;
            backgroundPanel.SizeScaleY = 1f;

            fillArea = new Panel("Fill Area");
            AddChild(fillArea);
            fillArea.BackgroundColor = Color.clear;
            fillArea.SizeOffsetX = SizeOffsetX;

            fill = new Panel("Fill");
            fillArea.AddChild(fill);
            fill.BackgroundColor = Color.black;
            fillColor = Color.black;
            fill.SizeOffsetY = SizeOffsetY;

            handleArea = new Panel("Handle Area");
            AddChild(handleArea);
            handleArea.BackgroundColor = Color.clear;
            handleArea.SizeOffsetX = SizeOffsetX;

            handle = new Panel("Handle");
            handleArea.AddChild(handle);
            handle.BackgroundColor = Color.black;
            handleColor = Color.black;
            handle.SizeOffsetX = SizeOffsetY * 1.5f;
            handle.SizeOffsetY = SizeOffsetY * 1.5f;
            handle.PositionOffsetX = handle.SizeOffsetX / -2;
            handle.PositionOffsetY = (SizeOffsetY - handle.SizeOffsetY) / 2;

            slider.fillRect = fill.GetRootObject().GetComponent<RectTransform>();
            slider.handleRect = handle.GetRootObject().GetComponent<RectTransform>();
            slider.targetGraphic = handle.GetRootObject().GetComponent<Image>();
        }

        public override void Update()
        {
            base.Update();
            value = slider.value;
        }
    }
}
